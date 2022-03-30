//Importations utiles
const WebSocket = require('ws');
const Room = require('./Room');
rooms = [];
let roomCourante;

//Création du serveur
const wss = new WebSocket.Server({port:3000}, () =>{
    console.log("Le serveur a bien démarré");
})

//Écoute du port
wss.on('listening', () => {
    console.log("Le serveur écoute sur le port 3000");
})

//Gestion des connections au serveur
wss.on('connection', (ws, req) => {

    var id = req.headers['sec-websocket-key'];
    console.log("Connection au serveur effectuée par " + id);

    //Lors de la réception d'un message venant de l'un des clients
    ws.on('message', (data) => {

        console.log(JSON.parse(data));
        dataJson = JSON.parse(data);

        //Suivant le type du message on effectue différentes actions
        console.log("Le type de message est " + dataJson.type);
        switch(dataJson.type){

            case "plateau":

                console.log(rooms);

                for(let i = 0; i < rooms.length; i++){

                    console.log(rooms[i].nom + " : " + dataJson.nomRoom);

                    if(rooms[i].nom == dataJson.nomRoom){
                        console.log("Mise à jour du plateau dans la room");
                        rooms[i].miseAJourDuPlateau(ws, data);
                    }
                }

                break;

            case "creationRoom":

                //On récupère les infos de la room
                let nomRoom = dataJson.nom;

                console.log(rooms.length);

                //Si la room possède le même nom qu'une autre, on ne la crée pas
                for(let i = 0; i < rooms.length; i++){
                    if(rooms[i].nom == nomRoom){
                        console.log("Une room porte déjà le nom " + nomRoom);
                        ws.send("Room");
                        return;
                    }
                }

                //Sinon, on l'ajoute à la liste
                let nombreDeJoueursRoom = dataJson.nombreDeJoueurs;

                console.log("Nom room : " + nomRoom);
                console.log("NB joueurs de la room : " + nombreDeJoueursRoom);

                let room = new Room(nomRoom, nombreDeJoueursRoom);

                room.ajouterUneConnection(ws);
                rooms.push(room);
                
                break;

            case "majRoom":

                roomsToSend = [];

                //Pour chaque rooms existante
                rooms.forEach(element => {

                    console.log(element.nombreCourantDeConnection());
                    console.log(element.nombreDeJoueurs);

                    //Si la room n'est pas pleine
                    if(element.nombreCourantDeConnection() < element.nombreDeJoueurs){

                        //On crée une room avec des paramètres particulier
                        let room = {
                            nom: element.nom,
                            nbJoueursTotal: element.nombreDeJoueurs,
                            nbJoueursCourant: element.nombreCourantDeConnection()
                        }

                        roomsToSend.push(room);
                    }
                    
                });

                console.log(roomsToSend);

                let requeteMiseAJourRoom = {
                    type: "affichageRooms",
                    rooms: roomsToSend
                }

                //On envoie toute la liste de rooms à tous les autres membres du serveur
                setTimeout(() => {  ws.send(JSON.stringify(requeteMiseAJourRoom)); }, 2000);           

                break;

            case "joinRoom":

                console.log(rooms);

                for(let i = 0; i < rooms.length; i++){
                    if(rooms[i].nom == dataJson.nomRoom){
                        console.log("La connexion veut rejoindre la room " + dataJson.nomRoom);
                        rooms[i].ajouterUneConnection(ws);
                        return;
                    }
                }
                break;

            case "finPartie":
                for(let i = 0; i < rooms.length; i++){
                    if(rooms[i].nom == dataJson.nomRoom){
                        roomCourante = dataJson.nomRoom;
                        rooms[i].finPartie();
                        return;
                    }
                }

                break;

            case "scores":

                for(let i = 0; i < rooms.length; i++){
                    if(rooms[i].nom == roomCourante){
                        rooms[i].envoiDesScores(ws);
                        return;
                    }
                }

                break;

            default:
                console.log("Message non identifié !");
                break;
        }

    })

    //Gestion lors de la déconnection d'un client
    ws.on('close', () => {
        console.log("Déconnection du serveur effectuée par " + id);
    })
})

//Gestion de la fermeture du serveur
wss.on('close', (ws) => {
    console.log("Le serveur a bien été éteint");
})
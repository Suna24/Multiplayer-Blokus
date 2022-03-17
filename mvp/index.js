//Importations utiles
const WebSocket = require('ws');
const Room = require('./Room');
rooms = [];

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

        dataJson = JSON.parse(data);

        //Suivant le type du message on effectue différentes actions
        switch(dataJson.type){

            case "plateau":

                for(let i = 0; i < rooms.lenght; i++){
                    if(rooms[i].id = dataJson.id){
                        rooms[i].miseAJourDuPlateau();
                    }
                }

                break;

            case "createRoom":

                //On récupère les infos de la room
                let nomRoom = dataJson.nom;

                //Si la room possède le même nom qu'une autre, on ne la crée pas
                for(let i = 0; i < rooms.lenght; i++){
                    if(rooms[i] == nomRoom){
                        return;
                    }
                }

                //Sinon, on l'ajoute à la liste
                let nombreDeJoueursRoom = dataJson.nombreDeJoueurs;
                rooms.push(new Room(nomRoom, nombreDeJoueursRoom, ws));
                
                break;

            default:
                break;
        }

    })

    //Gestion lors de la déconnection d'un client
    ws.on('close', () => {
        console.log("Déconnection du serveur effectuée par " + id);

        //On enlève le socket associé
        joueurs.splice(joueurs.indexOf(ws), 1);
    })
})

//Gestion de la fermeture du serveur
wss.on('close', (ws) => {
    console.log("Le serveur a bien été éteint");
})
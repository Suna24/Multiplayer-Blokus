//Importations utiles
const WebSocket = require('ws');
var nombreDeJoueurs = 0;
var tableauDeCouleurs = [1, 2, 3, 4];
var couleurDisponibles = [true, true, true, true];
var joueurs = [];
var indexDeParcoursDesJoueurs = 0;

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

    if(nombreDeJoueurs < 4){

        var id = req.headers['sec-websocket-key'];
        console.log("Connection effectuée par " + id);

        //On ajoute le joueur dans la liste des joueurs connectés
        joueurs.push(ws);

        var couleurJouee;
        nombreDeJoueurs = nombreDeJoueurs + 1;

        //On assigne une couleur disponible
        for(let i = 0; i < couleurDisponibles.length; i++){

            console.log("La couleur n° " + (i+1) + " est disponible : " + couleurDisponibles[i]);

            if(couleurDisponibles[i] == true){
                couleurDisponibles[i] = false;
                couleurJouee = tableauDeCouleurs[i];

                let requete = {
                    id : "joueur",
                    couleurJouee : couleurJouee
                }

                ws.send(JSON.stringify(requete));
                break;
            }
        }

        //Lors de la réception d'un message venant de l'un des clients
        ws.on('message', (data) => {

            let resultat = estJson(data);
            let requeteMiseAJourPlateau;

            if(resultat == false){

                console.log(id + " a envoyé un message : " + data);

            } else {
                console.log(JSON.parse(data));
                console.log(data);

                requeteMiseAJourPlateau = {
                    id : "plateau",
                    plateau : JSON.parse(data)
                }

                //C'est le tour du joueur suivant
                if(indexDeParcoursDesJoueurs == (joueurs.length - 1)){
                    indexDeParcoursDesJoueurs = 0;
                } else {
                    indexDeParcoursDesJoueurs++;
                }

                requeteUpdate = {
                    id: "tour",
                    tourCourant: false,
                    couleurTour: tableauDeCouleurs[indexDeParcoursDesJoueurs]
                }

                console.log(requeteUpdate);

                wss.clients.forEach(function each(client) {
                    if (client.readyState === WebSocket.OPEN && client === joueurs[indexDeParcoursDesJoueurs]) {

                    requeteTour = {
                        id: "tour",
                        tourCourant: true,
                        couleurTour: tableauDeCouleurs[indexDeParcoursDesJoueurs]
                    }

                    client.send(JSON.stringify(requeteTour));

                    } else {
                        client.send(JSON.stringify(requeteUpdate));
                    }
                });

            }

            //Envoi du message en broadcast
            wss.clients.forEach(function each(client) {
                if (client.readyState === WebSocket.OPEN) {
                  client.send(JSON.stringify(requeteMiseAJourPlateau));
                }
            });
        })

        //Gestion lors de la déconnection d'un client
        ws.on('close', () => {
            console.log("Déconnection effectuée par " + id);
            nombreDeJoueurs = nombreDeJoueurs - 1;

            //On enlève le socket associé
            joueurs.splice(joueurs.indexOf(ws), 1);
        })

    } else {
        console.log("Serveur full, veuillez attendre une déconnection");
    }

})

//Gestion de la fermeture du serveur
wss.on('close', (ws) => {
    console.log("Le serveur a bien été éteint");
})

//Regarde si une entrée est parsable ou non
function estJson(chaine) {
    try {
      var json = JSON.parse(chaine);
      return (typeof json === 'object');
    } catch (e) {
      return false;
    }
}
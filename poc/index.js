//Importations utiles
const WebSocket = require('ws');
var nombreDeJoueurs = 0;

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

        nombreDeJoueurs = nombreDeJoueurs + 1;

        ws.on('message', (data) => {

            console.log(id + " a envoyé un message : " + data);

            wss.clients.forEach(function each(client) {
                if (client.readyState === WebSocket.OPEN) {
                  client.send(data.toString());
                }
            });
        })

        ws.on('close', () => {
            console.log("Déconnection effectuée par " + id);
            nombreDeJoueurs = nombreDeJoueurs - 1;
        })

    } else {
        console.log("Serveur full, veuillez attendre une déconnection");
    }

})

//Gestion de la fermeture du serveur
wss.on('close', (ws) => {
    console.log("Le serveur a bien été éteint");
})
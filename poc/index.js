//Importations utiles
const WebSocket = require('ws');

const wss = new WebSocket.Server({port:3000}, () =>{
    console.log("Le serveur a bien démarré");
})

wss.on('listening', () => {
    console.log("Le serveur écoute sur le port 3000");
})

wss.on('connection', (ws) => {
    console.log("Connection effectuée");

    ws.on('close', () => {
        console.log("Déconnection effectuée");
    })

})

wss.on('close', (ws) => {
    console.log("Le serveur a bien été éteint");
})
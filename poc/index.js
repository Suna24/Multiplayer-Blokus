//Importations utiles
const {Server} = require('socket.io');
const port = Number(process.env.port) || 3000;

//Création du serveur et écoute sur un port
const io = new Server();
io.listen(port);

console.log('Le serveur à bien démarré et écoute sur le port ' + port);

//Gestion lors de la connection d'un joueur
io.on('connection', (socket) => {  
    console.log("Connecté ? : " + socket.connected);
    console.log("Connection effectuée par un joueur");

    socket.conn.on('close', (raison) => {  
        console.log("Déconnection effectuée par un joueur");
    })
})
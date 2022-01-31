//Importations utiles
const {Server} = require('socket.io');
const port = Number(process.env.port) || 3000;

//Création du serveur et écoute sur un port
const io = new Server();
io.listen(port);

console.log('Le serveur à bien démarré et écoute sur le port ' + port);

//Fermeture du serveur
io.close();

console.log('Le serveur a bien été fermé');
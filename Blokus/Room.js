const WebSocket = require('ws');

class Room {

    couleur = [];
    tableauDeCouleurs = [1, 2, 3, 4];
    couleurDisponibles = [true, true, true, true];
    indexDeParcoursDesJoueurs = 0;
    nom;
    nombreDeJoueurs;
    scores = [];

    constructor(nom, nombreDeJoueurs){
        this.nom = nom;
        this.nombreDeJoueurs = nombreDeJoueurs;
        this.connections = [];
    }

    ajouterUneConnection(ws){

        if(this.connections.length < this.nombreDeJoueurs){
            this.connections.push(ws);

            this.attributionCouleur(ws);

        } else {
            console.log("La room est pleine !");
        }

        console.log("Cbn de connections dans la room ? " + this.connections.length);

    }

    nombreCourantDeConnection(){
        return this.connections.length;
    }

    attributionCouleur(ws){
        var couleurJouee;

        //On assigne une couleur disponible
        for(let i = 0; i < this.couleurDisponibles.length; i++){

            console.log("La couleur n° " + (i+1) + " est disponible : " + this.couleurDisponibles[i]);

            if(this.couleurDisponibles[i] == true){
                this.couleurDisponibles[i] = false;
                couleurJouee = this.tableauDeCouleurs[i];

                let requete = {
                    type : 'joueur',
                    couleurJouee : couleurJouee,
                    nomRoom : this.nom
                }

                setTimeout(() => { ws.send(JSON.stringify(requete)); }, 2000);

                break;
            }
        }
    }

    miseAJourDuPlateau(webSocket, data){

        let dataJson = JSON.parse(data);
        console.log(dataJson);
        let plateau = dataJson.plateau;

        let index = this.connections.indexOf(webSocket);
        console.log(index);
        this.scores[index] = dataJson.score;
        console.log(this.scores);

        let requeteMiseAJourPlateau = {
            type : "plateau",
            plateau : plateau
        }

        //Envoi du message à toutes les connections de la room
        this.connections.forEach(ws => {
            if (ws.readyState === WebSocket.OPEN) {
                ws.send(JSON.stringify(requeteMiseAJourPlateau));
            }
        })

        this.miseAJourDuTour();
    }

    miseAJourDuTour(){

        //C'est le tour du joueur suivant
        if(this.indexDeParcoursDesJoueurs == (this.connections.length - 1)){
            this.indexDeParcoursDesJoueurs = 0;
        } else {
            this.indexDeParcoursDesJoueurs++;
        }

        let requeteTour = {
            type: "tour",
            tourCourant: true,
            couleurTour: this.tableauDeCouleurs[this.indexDeParcoursDesJoueurs]
        }

        //Envoi du message à toutes les connections de la room
        this.connections.forEach(client => {
            if (client.readyState === WebSocket.OPEN && client === this.connections[this.indexDeParcoursDesJoueurs]) {
                client.send(JSON.stringify(requeteTour));
            }
        })
    }

    envoiDesScores(ws){

        let requeteScore = {
            type: "scores",
            scores: this.scores
        }

        console.log(requeteScore);

        //Envoi du message à la connexion de la room
        setTimeout(() => { ws.send(JSON.stringify(requeteScore)); }, 2000);
    }

    finPartie(){

        let requete = {
            type: "finPartie"
        }

        this.connections.forEach(client => {
            if (client.readyState === WebSocket.OPEN) {
                 client.send(JSON.stringify(requete));
            }
        })
    }

}

module.exports = Room;
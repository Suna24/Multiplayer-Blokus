const WebSocket = require('ws');

class Room {

    connections = [];
    couleur = [];
    tableauDeCouleurs = [1, 2, 3, 4];
    couleurDisponibles = [true, true, true, true];
    indexDeParcoursDesJoueurs = 0;

    constructor(nom, nombreDeJoueurs, connections){
        this.nom = nom;
        this.nombreDeJoueurs = nombreDeJoueurs;
        this.connections = connections;
    }

    ajouterUneConnection(ws){

        if(this.connections.length < this.nombreDeJoueurs){
            this.connections.push(ws);

            attributionCouleur(ws);

        } else {
            console.log("La room est pleine !");
        }

    }

    attributionCouleur(ws){
        var couleurJouee;

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
    }

    miseAJourDuPlateau(){
        this.connections.forEach(ws => {

            ws.on('message', (data) => {

                //On transforme le message en JSON
                dataJSON = JSON.parse(data);
                plateau = dataJSON.plateau;

                let requeteMiseAJourPlateau = {
                    id : "plateau",
                    plateau : plateau
                }

                //Envoi du message à toutes les connections de la room
                this.connections.forEach(ws => {
                    if (ws.readyState === WebSocket.OPEN) {
                        ws.send(JSON.stringify(requeteMiseAJourPlateau));
                    }
                })

            })
    
        })

        this.miseAJourDuTour();
    }

    miseAJourDuTour(){
        this.connections.forEach(ws => {

            ws.on('message', (data) => {

                //C'est le tour du joueur suivant
                if(indexDeParcoursDesJoueurs == (joueurs.length - 1)){
                    indexDeParcoursDesJoueurs = 0;
                } else {
                    indexDeParcoursDesJoueurs++;
                }

                let requeteTour = {
                    id: "tour",
                    tourCourant: true,
                    couleurTour: tableauDeCouleurs[indexDeParcoursDesJoueurs]
                }

                //Envoi du message à toutes les connections de la room
                this.connections.forEach(client => {
                    if (client.readyState === WebSocket.OPEN && client === joueurs[indexDeParcoursDesJoueurs]) {
                        client.send(JSON.stringify(requeteTour));
                    }
                })

            })
    
        })
    }

}

module.exports = Room;
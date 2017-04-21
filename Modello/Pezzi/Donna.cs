using System;
using System.Collections.Generic;
using System.Linq;

namespace Scacchi.Modello.Pezzi
{
    public class Donna : IPezzo
    {
        private readonly Colore colore;
        public Donna(Colore colore)
        {

            this.colore = colore;
        }
        public Colore Colore
        {
            get
            {
                return colore;
            }
        }
        public bool PuòMuovere(
            Colonna colonnaPartenza,
            Traversa traversaPartenza,
            Colonna colonnaArrivo,
            Traversa traversaArrivo,
            IEnumerable<ICasa> listaCase = null)
        {

            listaCase = listaCase ?? Enumerable.Empty<ICasa>();
            //prendo la casa di partenza
            ICasa casaPartenza = listaCase.SingleOrDefault(casa => casa.Colonna == colonnaPartenza
             && casa.Traversa == traversaPartenza
             && casa.PezzoPresente == this);
            //prendo la casa di arrivo senza specificare se ci sia un pezzo o meno
            ICasa casaArrivo = listaCase.SingleOrDefault(casa => casa.Colonna == colonnaArrivo
                         && casa.Traversa == traversaArrivo);
            //controllo che non ci siano pezzi tra me e l'arrivo              
            //prendo le differenze tra colonne e traverse            
            var differenzaColonne = (int)colonnaPartenza - (int)colonnaArrivo;
            var differenzaTraverse = (int)traversaPartenza - (int)traversaArrivo;
            //confronto le colonne e le traverse
            var stessaColonna = colonnaPartenza == colonnaArrivo;
            var stessaTraversa = traversaPartenza == traversaArrivo;
            //controllo se sono rimasto fermo
            if (differenzaColonne == 0 && differenzaTraverse == 0)
                return false;
            //prima di tutto devo capire se mi sto muovendo verticalmente, orizzontalmente o in obliquo
            if ((Math.Abs(differenzaColonne) - Math.Abs(differenzaTraverse)) == 0)
            {
                //mi sto muovendo in obliquo
                //controllo che davanti a me non ci siano pezzi di alcun genere
                int pezziTrovati = (listaCase.Where(casa => casa.Colonna != casaPartenza.Colonna
                     && casa.Traversa != casaPartenza.Traversa && casa.PezzoPresente != null)).Count();
                if (pezziTrovati > 1)
                {
                    //ci sono più di un pezzo tra me e la destinazione, non è possibile andare oltre!
                    return false;
                }
                else if(pezziTrovati==1){
                    //devo controllare la posizione di questo pezzo
                    if(casaArrivo.PezzoPresente==null){
                        //allora il pezzo sta in mezzo tra me e la destinazione 
                        return false;
                    }
                    else{
                        return true;
                    }
                }
                else if (pezziTrovati==0){
                    //non c'è nessun pezzo tra me e la destinazione posso andare lì
                    return true;
                }
                return false;

            }
            else if (((stessaTraversa && !stessaColonna){

            }
            else if(stessaColonna && !stessaTraversa)){

            }
        }
    }
}
using System;
using System.Collections.Generic;

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
            var differenzaColonne = colonnaPartenza - colonnaArrivo;
            var differenzaTraverse = (int)traversaPartenza - (int)traversaArrivo;
            var stessaColonna = colonnaPartenza == colonnaArrivo;
            var stessaTraversa = traversaPartenza == traversaArrivo;
            if (differenzaColonne == 0 && differenzaTraverse == 0)
                return false;
            if (((stessaTraversa && !stessaColonna)
            || (stessaColonna && !stessaTraversa)) 
            || ((Math.Abs(differenzaColonne) - Math.Abs(differenzaTraverse)) == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
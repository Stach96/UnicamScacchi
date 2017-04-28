using System;
using System.Collections.Generic;
using System.Linq;
using Scacchi.Modello;
using Scacchi.Modello.Pezzi;

namespace Scacchi.Modello
{
    public class Tavolo : ITavolo
    {
        public Tavolo(IScacchiera scacchiera, IOrologio orologio)
        {
            Scacchiera = scacchiera;
            Orologio = orologio;
        }
        public Dictionary<Colore, IGiocatore> Giocatori { get; private set; }

        public IScacchiera Scacchiera { get; private set; }
        public IOrologio Orologio { get; private set; }

        public void AvviaPartita()
        {
            if (Giocatori == null)
                throw new InvalidOperationException("Attenzione, prima devi indicare i nomi dei giocatori!");
            Orologio.Accendi();
            Orologio.Avvia();
        }

        public void InserisciMossa(string mossa)
        {
            controllaSeTempoScaduto();
            Coordinata partenza = InterpretaCoordinataCasa(mossa.Substring(0, 2));
            controllaCasaPartenza(partenza);
            Coordinata arrivo = InterpretaCoordinataCasa(mossa.Substring(3, 2));
            controllaMovimentoPezzo(partenza, arrivo);
            //la mossa può essere accettata
            //salvo il pezzo dentro la casa di partenza
            IPezzo pezzo = Scacchiera[partenza.Colonna, partenza.Traversa].PezzoPresente;
            //sposto il pezzo in una casa, se vi era un pezzo dell'avversario ovviamente
            //verrà mangiato e sparirà dalla scacchiera!
            Scacchiera[partenza.Colonna, partenza.Traversa].PezzoPresente = null;
            Scacchiera[arrivo.Colonna, arrivo.Traversa].PezzoPresente = pezzo;
            controllaReMancante();
            //ora ho controllato davvero tutto, posso cambiare il turno dell'orologio!
            Orologio.FineTurno();
        }

        private void controllaReMancante()
        {
            IEnumerable<ICasa> caseConRe = Scacchiera.Case.Where(casa => casa.PezzoPresente?.GetType() == typeof(Re));
            if (caseConRe.Count() < 2 && caseConRe.Count() > 0)
            {
                //c'è un vincitore
                ICasa casaConRe = caseConRe.FirstOrDefault();
                if (casaConRe.PezzoPresente.Colore == Colore.Nero)
                {
                    Console.WriteLine($"Il vincitore è il giocatore: {this.Giocatori[Colore.Nero].Nome }");
                    Environment.Exit(0);
                }
                else if (casaConRe.PezzoPresente.Colore == Colore.Bianco)
                {
                    Console.WriteLine($"Il vincitore è il giocatore: {this.Giocatori[Colore.Bianco].Nome }");
                    Environment.Exit(0);
                }
            }
        }

        private void controllaMovimentoPezzo(Coordinata partenza, Coordinata arrivo)
        {
            var pezzoDaMuovere = Scacchiera[partenza.Colonna, partenza.Traversa].PezzoPresente;
            bool esito = pezzoDaMuovere.PuòMuovere(partenza.Colonna, partenza.Traversa,
             arrivo.Colonna, arrivo.Traversa, Scacchiera.Case);
            if (!esito)
            {
                throw new ArgumentException("Mossa non valida!");
            }
        }

        private void controllaSeTempoScaduto()
        {
            if (Orologio.TempoResiduoBianco <= TimeSpan.FromMinutes(0) && Orologio.TurnoAttuale == Colore.Bianco)
            {
                Console.WriteLine($"Il vincitore è il giocatore: {this.Giocatori[Colore.Nero].Nome }");
                Environment.Exit(0);
            }
            else if ((Orologio.TempoResiduoNero <= TimeSpan.FromMinutes(0) && Orologio.TurnoAttuale == Colore.Nero))
            {
                Console.WriteLine($"Il vincitore è il giocatore: {this.Giocatori[Colore.Bianco].Nome }");
                Environment.Exit(0);

            }
        }

        private void controllaCasaPartenza(Coordinata partenza)
        {
            ICasa casaPartenza = Scacchiera[partenza.Colonna, partenza.Traversa];
            if (casaPartenza.PezzoPresente.Colore != Orologio.TurnoAttuale)
            {
                Console.WriteLine("Non puoi muovere un pezzo non tuo!");
            }

        }

        internal Coordinata InterpretaCoordinataCasa(string casa)
        {
            Enum.TryParse<Colonna>(casa.Substring(0, 1), out Colonna colonna);
            int.TryParse(casa.Substring(1, 1), out int traversaInt);
            Traversa traversa = (Traversa)traversaInt;
            return new Coordinata(traversa, colonna);
        }


        public void RiceviGiocatori(string nomeBianco, string nomeNero)
        {
            Giocatori = new Dictionary<Colore, IGiocatore>();
            Giocatori.Add(Colore.Bianco, new Giocatore(nomeBianco));
            Giocatori.Add(Colore.Nero, new Giocatore(nomeNero));
        }
    }
}

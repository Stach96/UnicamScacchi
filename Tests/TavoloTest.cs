using System.Collections.Generic;
using System.Linq;
using Scacchi.Modello;
using Scacchi.Modello.Pezzi;
using Xunit;

namespace Scacchi.Tests
{
    public class TavoloTest
    {
        [Fact]
        public void TestIlTavoloHaDueGiocatori()
        {
            ITavolo tavolo = new Tavolo(null, null);

            tavolo.RiceviGiocatori("Bianco", "Nero");

            Dictionary<Colore, IGiocatore> dict = tavolo.Giocatori;
            IGiocatore bianco = dict[Colore.Bianco];
            IGiocatore nero = dict[Colore.Nero];

            Assert.Equal(2, dict.Count);

            Assert.Equal(1, dict.Where(colore => colore.Key == Colore.Bianco).Count());
            Assert.Equal(1, dict.Where(colore => colore.Key == Colore.Nero).Count());

            Assert.Equal("Bianco", bianco.Nome);
            Assert.Equal("Nero", nero.Nome);

        }

        [Fact]
        public void AllAvvioDellaPartitaDeveEsserePredispostaUnaScacchieraELOrologioAvviato()
        {
            //Given
            IScacchiera scacchiera = new Scacchiera();
            IOrologio orologio = new Orologio();
            ITavolo tavolo = new Tavolo(scacchiera, orologio);

            //When
            tavolo.RiceviGiocatori("", "");
            tavolo.AvviaPartita();

            //Then
            Assert.NotEqual(null, tavolo.Scacchiera);
            Assert.NotEqual(null, tavolo.Orologio);
            Assert.False(orologio.InPausa);
        }

        [Fact]
        public void IlTavoloDeveEssereInGradoDiInterpretareLeCoordinateDigitateDallUtente()
        {
            //Given
            Tavolo tavolo = new Tavolo(null, null);
            //When
            Coordinata coordinata = tavolo.InterpretaCoordinataCasa("A4");

            //Then
            Assert.Equal(Traversa.Quarta, coordinata.Traversa);
            Assert.Equal(Colonna.A, coordinata.Colonna);
        }

        [Fact]
        public void UnaMossaValidaDeveEssereAccettata()
        {
            //Given
            IScacchiera scacchiera = new Scacchiera();

            IOrologio orologio = new Orologio();

            ITavolo tavolo = new Tavolo(scacchiera, orologio);

            tavolo.RiceviGiocatori("Lorenzo", "Negro");
            tavolo.AvviaPartita();
            //prendo il pezzo nella posizione di partenza
            IPezzo pezzo = scacchiera[Colonna.A, Traversa.Seconda].PezzoPresente;
            //When
            tavolo.InserisciMossa("A2 A3");
            //Then
            //controllo che la posizione a3 contenga un pezzo
            Assert.NotNull(scacchiera[Colonna.A, Traversa.Terza]);
            Assert.Equal(pezzo.Colore, scacchiera[Colonna.A, Traversa.Terza].PezzoPresente.Colore);
            Assert.Equal(pezzo.GetType(), scacchiera[Colonna.A, Traversa.Terza].PezzoPresente.GetType());
        }
    }
}
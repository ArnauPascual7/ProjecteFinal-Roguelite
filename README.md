# ProjecteFinal-Roguelite

### Arnau Pascual - Aleix Tuneu - Lluc Velázquez

## Build

La build es troba a 

## Controls

WASD - Mou el jugador

Left Click - Dispara

Shift/Right Click - Evasió

1/Up Scroll - Canvia a la primera arma

2/Down Scroll - Canvia a la segona arma

## Sistema de Jugador

El jugador es pot moure per un espai 2d en tercera persona i amb una perspectiva top-down, per moure's pot caminar i fer un "dash" per avançar molt ràpid en una direcció.

Hi ha sistema de vida tant per al jugador com pels enemics, el jugador te vida representada en cors.

El jugador compta amb dues armes, una amb un comportament de pistola i una altra de fusell d'assalt. Els enemics ataquen cos a cos, i llencen fletxes cap al jugador.

## Combat i mecàniques bàsiques

El jugador porta dues armes màgiques amb les quals pot atacar als enemics amb els que es troba al llarg de la masmorra. L'arma amb comportament de pistola infligeix 100 de mal per cop i dispara 1 projectil per atac, mentre que la de fussell infligeix 150 de mal per cop i dispara 2 projectils per atac.

El jugador té 3 cors de vida i pot tenir cors per la meitat de vida, per exemple 2.5, que són 2 cors i mig. També aquest té 5 de velocitat.

Els enemics porten diferents armes i tenen vida i velocitats diferents. Aquest tenen un valor base de vida de 300 i pot variar 300 més, fent que tingui entre 1 i 600 de vida, mentre que la velocitat té un valor base de 3 i pot variar 3 més, fent que tingui entre 0 i 6 de velocitat.

Els enemics poden portar 4 tipus d'armes que varien en atac i mal:
- Espasa curta: 1 de mal cos a cos
- Llança: 1.5 de mal cos a cos
- Espasa llarga: 2 de mal cos a cos
- Arc: 1 de mal a distància

Tots els enemics que es mouen persegueixen al jugador i quan el perden de vista es queden quiets en el lloc on l'han perdut.

## Level design i flux del joc

El joc comença en una sala amb 4 portes en les 4 direccions, el jugador pot anar cap a qualsevol direcció per explorar la masmorra. A mesura que s'explora la masmorra es troben diferents enemics que persegueixen al jugador o el disparen des de lluny. En morir i obtenir monedes matant monstres, aquestes es poden invertir en millorar el personatge per poder avançar més en la masmorra.

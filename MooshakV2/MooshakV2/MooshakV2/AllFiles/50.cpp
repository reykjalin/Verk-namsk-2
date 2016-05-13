#include <iostream>
#include "Card.h"
#include "MageCard.h"
#include "PriestCard.h"

using namespace std;

int main()
{
    MageCard card1("Unstable Portal",
                   2,
                   "spell",
                   "Add a random minion to your hand. It costs (3) less.");

    PriestCard card2 ("Northshire Cleric",
                      1,
                      "minion",
                      "Whenever a minion is healed, draw a card.");

    PriestCard card3 ("Cabal Shadow Priest",
                      6,
                      "minion",
                      "Take control of a minion that has 2 or less attack.");

    card1.printCard();
    card2.printCard();
    card3.printCard();

    return 0;
}

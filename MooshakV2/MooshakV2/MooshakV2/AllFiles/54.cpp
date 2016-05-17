#include "PriestCard.h"
#include <iostream>
using namespace std;

PriestCard::PriestCard() : Card(), effect("empty"){ }

PriestCard::PriestCard(string _name, int _mana, string _type, string _effect)
                        : Card(_name, _mana, _type), effect(_effect) {}

string PriestCard::getEffect() const{
    return effect;
}

void PriestCard::printCard() {
    cout << "Name: " << name << endl;
    cout << mana << " mana " << type << "." << endl;
    cout << effect << endl;
    cout << endl;
}

#include "MageCard.h"
#include <iostream>
using namespace std;

MageCard::MageCard() : Card(), effect("empty"){ }

MageCard::MageCard(string _name, int _mana, string _type, string _effect)
                        : Card(_name, _mana, _type), effect(_effect) {}

string MageCard::getEffect() const{
    return effect;
}

void MageCard::printCard() {
    cout << "Name: " << name << endl;
    cout << mana << " mana " << type << "." << endl;
    cout << effect << endl;
    cout << endl;
}

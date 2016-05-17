#include "Card.h"
#include <iostream>
using namespace std;

Card::Card() {

}

Card::Card(string _name, int _mana, string _type) {
    name = _name;
    mana = _mana;
    type = _type;
}

string Card::getName() const {
    return name;
}

int Card::getMana() const {
    return mana;
}

string Card::getType() const {
    return type;
}

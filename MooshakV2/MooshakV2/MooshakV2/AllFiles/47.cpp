#include <iostream>
#include "jumpit.h"
using namespace std;

const int PENALTY = 1000;	// Used to assign a very high cost

int jumpIt(const int board[], int startIndex, int endIndex) {

    if (startIndex == endIndex) {
        return board[endIndex];
    }

    if(startIndex > endIndex) {
        return PENALTY;
    }

    int cost1 = board[startIndex] + jumpIt(board, (startIndex + 1), endIndex);
    int cost2 = board[startIndex] + jumpIt(board, (startIndex + 2), endIndex);

    return min(cost1, cost2);
}


pragma(optimize,true).

member(Item,[Item|Items]).
member(Item,[AnotherItem|Items]) :-
    member(Item,Items).

next_to(X,Y,List) :-
    is_right(X,Y,List).
next_to(X,Y,List) :-
    is_right(Y,X,List).


is_right(L,R,[L,R|X1]).
is_right(L,R,[X1|Rest]) :-
    is_right(L,R,Rest).


/// Solves the zebra problem.
///
/// See http://en.literateprograms.org/Zebra_Puzzle_(Prolog)
///
owns_zebra(Street,Who) :-
    (Street = [House1,House2,House3,House4,House5]),
    member(house(red,englishman,P1,D1,S1),Street),
    member(house(C2,spaniard,dog,D2,S2),Street),
    member(house(green,N3,P3,coffee,S3),Street),
    member(house(C4,ukrainian,P4,tea,S4),Street),
    is_right(house(green,N5a,P5a,D5a,S5a),house(ivory,N5b,P5b,D5b,S5b),Street),
    member(house(C6,N6,snails,D6,old_gold),Street),
    member(house(yellow,N7,P7,D7,kools),Street),
    ([H8a,H8b,house(C8,N8,P8,milk,S8),H8d,H8e] = Street),
    ([house(C9,norwegian,P9,D9,S9)|Others9] = Street),
    next_to(house(C10a,N10a,P10a,D10a,chesterfields),house(C10b,N10b,fox,D10b,S10b),Street),
    next_to(house(C11a,N11a,P11a,D11a,kools),house(C11b,N11b,horse,D11b,S11b),Street),
    member(house(C12,N12,P12,orange_juice,lucky_strike),Street),
    member(house(C13,japanese,P13,D13,parliaments),Street),
    next_to(house(C14a,norwegian,P14a,D14a,S14a),house(blue,N14b,P14b,D14b,S14b),Street),
    member(house(C14,Who,zebra,D14,S14),Street).

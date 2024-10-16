pragma(optimize,true).


cargo(goat).
cargo(chicken).
cargo(seed).


containsItem([Item|V],Item).
containsItem([OtherItem|List],Item) :-
    (OtherItem \= Item),
    containsItem(List,Item).


excludesItem([],Item).
excludesItem([OtherItem|List],Item) :-
    (OtherItem \= Item),
    excludesItem(List,Item).


removedFrom([],[],Item).
removedFrom(NewList,[NewItem|List],Item) :-
    (NewItem = Item),
    removedFrom(NewList,List,Item).
removedFrom([NewItem|NewList],[NewItem|List],Item) :-
    (NewItem \= Item),
    removedFrom(NewList,List,Item).


safe([]).
safe([goat]).
safe([chicken]).
safe([seed]).
safe([goat,seed]).
safe([seed,goat]).
safe([goat,chicken,seed]).
safe([goat,seed,chicken]).
safe([chicken,goat,seed]).
safe([seed,goat,chicken]).
safe([chicken,seed,goat]).
safe([seed,chicken,goat]).


validMove(NewState,Cargo,State) :-
    containsItem(State,farmer),
    removedFrom(StateWithoutFarmer,State,farmer),
    cargo(Cargo),
    containsItem(StateWithoutFarmer,Cargo),
    removedFrom(NewState,StateWithoutFarmer,Cargo),
    safe(NewState).


validMove(StateWithoutFarmer,State) :-
    containsItem(State,farmer),
    removedFrom(StateWithoutFarmer,State,farmer),
    safe(StateWithoutFarmer).


startState(state([farmer,goat,chicken,seed],[])).


stopState(state([],X)).


possibleMove(NewState,[state(L,R)|States]) :-
    validMove(NewL,Cargo,L),
    (state(NewL,[farmer,Cargo|R]) = NewState),
    excludesItem(States,NewState).
possibleMove(NewState,[state(L,R)|States]) :-
    validMove(NewR,Cargo,R),
    (state([farmer,Cargo|L],NewR) = NewState),
    excludesItem(States,NewState).
possibleMove(NewState,[state(L,R)|States]) :-
    validMove(NewL,L),
    (state(NewL,[farmer|R]) = NewState),
    excludesItem(States,NewState).
possibleMove(NewState,[state(L,R)|States]) :-
    validMove(NewR,R),
    (state([farmer|L],NewR) = NewState),
    excludesItem(States,NewState).


solve(States,States) :-
    ([S|Ss] = States),
    stopState(S).
solve(States,FinalStates) :-
    possibleMove(NextState,States),
    solve([NextState|States],FinalStates).


solve(FinalStates) :-
    startState(State),
    solve([State],FinalStates).

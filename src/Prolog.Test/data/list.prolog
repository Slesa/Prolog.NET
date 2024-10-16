pragma(optimize,true).


/// append (ResultList, List1, List2)
/// ResultList = List1 + List2
/// List1 + List2 = ResultList
/// 
append(List,[],List).
append([Item|Result],[Item|List1],List2) :-
    append(Result,List1,List2).


/// appendItem (ResultList, List, Item)
/// ResultList = List + Item
/// List + Item = ResultList
/// 
appendItem(ResultList,List,Item) :-
    append(ResultList,List,[Item]).


/// join (List, Left, Pivot, Right)
/// List = Left + Pivot + Right
/// 
join(List,Left,Pivot,Right) :-
    appendItem(L,Left,Pivot),
    append(List,L,Right).


/// Member (Item, List)
/// Item = member of List
/// 
member(Item,List) :-
    split(L,Item,R,List).


/// permute (Result, Items)
/// Result = permutation of Items
/// 
permute([Item],[Item]).
permute(Result,[Item|Items]) :-
    permute(P,Items),
    append(P,Left,Right),
    appendItem(R,Left,Item),
    append(Result,R,Right).


/// size (Size, List)
/// Size = size of List
/// List = list of Size
/// 
size(0,[]).
size(Size,[Item|Items]) :-
    size(S,Items),
    (Size := (S + 1)).


/// split (Left, Pivot, Right, List)
/// Left + Pivot + Right = List
/// 
split(Left,Pivot,Right,List) :-
    append(List,L,Right),
    appendItem(L,Left,Pivot).


/// reverse (Result, Items)
/// Result = reverse of Items
/// 
reverse([],[]).
reverse(Result,[Item|Items]) :-
    reverse(R,Items),
    appendItem(Result,R,Item).


/// prefix (Prefix, Items)
/// Prefix = prefix of Items
/// 
prefix(Prefix,Items) :-
    append(Items,Prefix,L).


/// suffix (Suffix, Items)
/// Suffix = suffix of Items
/// 
suffix(Suffix,Items) :-
    append(Items,L,Suffix).


/// Partition (LessEqual, Greater, Pivot, Items)
/// LessEqual = members of Items <= Pivot
/// Greater = members of Items > Pivot
/// 
partition([],[],Pivot,[]).
partition([Item|LessEqual],Greater,Pivot,[Item|Items]) :-
    (Item =< Pivot),
    partition(LessEqual,Greater,Pivot,Items).
partition(LessEqual,[Item|Greater],Pivot,[Item|Items]) :-
    (Item > Pivot),
    partition(LessEqual,Greater,Pivot,Items).


/// qsort (SortedItems, Items)
/// SortedItems = sorted list of Items
/// 
qsort([],[]).
qsort(Result,[Item|Items]) :-
    partition(LE,G,Item,Items),
    qsort(SortedLE,LE),
    qsort(SortedG,G),
    join(Result,SortedLE,Item,SortedG).


/// sequence (Items, Size)
/// Items = list of integers between 1 and Size, inclusive
/// 
sequence(Items,Size) :-
    sequence(Items,1,Size).


/// divide (Left, Right, Items, Count)
/// Left = first Count items of Items
/// Right = remaining items of Items
/// 
divide([],Items,Items,0).
divide([Item|Left],Right,[Item|Items],Count) :-
    greater(Count,0),
    (SubCount is subtract(Count,1)),
    divide(Left,Right,Items,SubCount).


/// shuffle (Result, Items, Count)
/// Result = Items shuffled Count times
/// 
shuffle(Items,Items,0).
shuffle(Result,Items,Count) :-
    greater(Count,0),
    (SubCount is subtract(Count,1)),
    shuffle(S,Items,SubCount),
    size(Size,Items),
    random(0,Size,R),
    divide(Left,Right,S,R),
    merge(Result,Right,Left).


/// merge (Result, Left, Right)
/// Result = merged results of Left and Right
/// 
merge([LeftItem,RightItem|Items],[LeftItem|Left],[RightItem|Right]) :-
    merge(Items,Left,Right).
merge([],[],[]).
merge(Items,Items,[]) :-
    size(Size,Items),
    greater(Size,0).
merge(Items,[],Items) :-
    size(Size,Items),
    greater(Size,0).


/// sequence (Items, Min, Max)
/// Items = list of integers between Min and Max, inclusive
/// 
sequence([Min|Items],Min,Max) :-
    less(Min,Max),
    (MinPlus is add(Min,1)),
    sequence(Items,MinPlus,Max).
sequence([MinMax],MinMax,MinMax).


test(X,Y,Z) :-
    sequence(X,10),
    shuffle(Y,X,10),
    qsort(Z,Y).

pragma(optimize,true).


/// append (ResultList, List1, List2)
/// ResultList = List1 + List2
/// List1 + List2 = ResultList
/// 
append(List,[],List).
append([Item|Result],[Item|List1],List2) :-
    append(Result,List1,List2).

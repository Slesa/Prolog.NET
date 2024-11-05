day(monday).
day(tuesday).
day(wednesday).
day(thursday).
day(friday).


shift(first).
shift(second).
shift(third).


workPeriod(workPeriod(Day,Shift)) :-
    day(Day),
    shift(Shift).


shiftAssignment(alice,first).
shiftAssignment(bob,second).
shiftAssignment(cathy,third).
shiftAssignment(doug,first).
shiftAssignment(emily,second).
shiftAssignment(fred,third).
shiftAssignment(greta,first).
shiftAssignment(henry,second).
shiftAssignment(ina,third).
shiftAssignment(jerry,first).
shiftAssignment(kelly,second).
shiftAssignment(larry,third).
shiftAssignment(mary,first).
shiftAssignment(ned,second).
shiftAssignment(olive,third).
shiftAssignment(peter,first).
shiftAssignment(quinn,second).
shiftAssignment(roger,third).
shiftAssignment(sally,first).
shiftAssignment(teddy,second).
shiftAssignment(ursula,third).
shiftAssignment(vincent,first).
shiftAssignment(wanda,second).
shiftAssignment(xavier,third).
shiftAssignment(yvonne,first).
shiftAssignment(zack,second).


dayAssignment(alice,[monday,tuesday]).
dayAssignment(bob,[tuesday,wednesday]).
dayAssignment(cathy,[wednesday,thursday]).
dayAssignment(doug,[thursday,friday]).
dayAssignment(emily,[friday,monday]).
dayAssignment(fred,[monday,tuesday]).
dayAssignment(greta,[tuesday,wednesday]).
dayAssignment(henry,[wednesday,thursday]).
dayAssignment(ina,[thursday,friday]).
dayAssignment(jerry,[friday,monday]).
dayAssignment(kelly,[monday,tuesday]).
dayAssignment(larry,[tuesday,wednesday]).
dayAssignment(mary,[wednesday,thursday]).
dayAssignment(ned,[thursday,friday]).
dayAssignment(olive,[friday,monday]).
dayAssignment(peter,[monday,tuesday]).
dayAssignment(quinn,[tuesday,wednesday]).
dayAssignment(roger,[wednesday,thursday]).
dayAssignment(sally,[thursday,friday]).
dayAssignment(teddy,[friday,monday]).
dayAssignment(ursula,[monday,tuesday]).
dayAssignment(vincent,[tuesday,wednesday]).
dayAssignment(wanda,[wednesday,thursday]).
dayAssignment(xavier,[thursday,friday]).
dayAssignment(yvonne,[friday,monday]).
dayAssignment(zack,[monday,tuesday]).


workPeriodAssignment(workPeriodAssignment(Person,workPeriod(Day,Shift))) :-
    dayAssignment(Person,Days),
    containsItem(Days,Day),
    shiftAssignment(Person,Shift).


containsItem([Item|Items],Item).
containsItem([AnotherItem|List],Item) :-
    containsItem(List,Item).


workPeriodAssignments([],[]).
workPeriodAssignments([workPeriodAssignment(Person,WorkPeriod)|Assignments],[WorkPeriod|WorkPeriods]) :-
    workPeriodAssignments(Assignments,WorkPeriods),
    workPeriodAssignment(workPeriodAssignment(Person,WorkPeriod)).


solve(Assignments) :-
    findall(WorkPeriod,workPeriod(WorkPeriod),WorkPeriods),
    workPeriodAssignments(Assignments,WorkPeriods).

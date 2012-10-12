pragma(optimize,true).

/// move(FromBoard, ToBoard)
/// Defines possible peg jobs.  
/// o  0 indicates the hole is empty.  
/// o  1 indicates the hole is occupied by a peg.
/// o  Variables indicate holes not affected by the jump.
///
move(board(1,1,P2,0,P4,P5,P6,P7,P8,P9,P10,P11,P12,P13,P14),board(0,0,P2,1,P4,P5,P6,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(1,P1,1,P3,P4,0,P6,P7,P8,P9,P10,P11,P12,P13,P14),board(0,P1,0,P3,P4,1,P6,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,1,P2,1,P4,P5,0,P7,P8,P9,P10,P11,P12,P13,P14),board(P0,0,P2,0,P4,P5,1,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,1,P2,P3,1,P5,P6,P7,0,P9,P10,P11,P12,P13,P14),board(P0,0,P2,P3,0,P5,P6,P7,1,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,1,P3,1,P5,P6,0,P8,P9,P10,P11,P12,P13,P14),board(P0,P1,0,P3,0,P5,P6,1,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,1,P3,P4,1,P6,P7,P8,0,P10,P11,P12,P13,P14),board(P0,P1,0,P3,P4,0,P6,P7,P8,1,P10,P11,P12,P13,P14)).
move(board(0,1,P2,1,P4,P5,P6,P7,P8,P9,P10,P11,P12,P13,P14),board(1,0,P2,0,P4,P5,P6,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,1,1,0,P6,P7,P8,P9,P10,P11,P12,P13,P14),board(P0,P1,P2,0,0,1,P6,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,1,P4,P5,P6,1,P8,P9,P10,P11,0,P13,P14),board(P0,P1,P2,0,P4,P5,P6,0,P8,P9,P10,P11,1,P13,P14)).
move(board(P0,P1,P2,1,P4,P5,1,P7,P8,P9,0,P11,P12,P13,P14),board(P0,P1,P2,0,P4,P5,0,P7,P8,P9,1,P11,P12,P13,P14)).
move(board(P0,P1,P2,P3,1,P5,P6,1,P8,P9,P10,0,P12,P13,P14),board(P0,P1,P2,P3,0,P5,P6,0,P8,P9,P10,1,P12,P13,P14)).
move(board(P0,P1,P2,P3,1,P5,P6,P7,1,P9,P10,P11,P12,0,P14),board(P0,P1,P2,P3,0,P5,P6,P7,0,P9,P10,P11,P12,1,P14)).
move(board(0,P1,1,P3,P4,1,P6,P7,P8,P9,P10,P11,P12,P13,P14),board(1,P1,0,P3,P4,0,P6,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,0,1,1,P6,P7,P8,P9,P10,P11,P12,P13,P14),board(P0,P1,P2,1,0,0,P6,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,P3,P4,1,P6,P7,1,P9,P10,P11,0,P13,P14),board(P0,P1,P2,P3,P4,0,P6,P7,0,P9,P10,P11,1,P13,P14)).
move(board(P0,P1,P2,P3,P4,1,P6,P7,P8,1,P10,P11,P12,P13,0),board(P0,P1,P2,P3,P4,0,P6,P7,P8,0,P10,P11,P12,P13,1)).
move(board(P0,0,P2,1,P4,P5,1,P7,P8,P9,P10,P11,P12,P13,P14),board(P0,1,P2,0,P4,P5,0,P7,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,1,1,0,P9,P10,P11,P12,P13,P14),board(P0,P1,P2,P3,P4,P5,0,0,1,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,0,P3,1,P5,P6,1,P8,P9,P10,P11,P12,P13,P14),board(P0,P1,1,P3,0,P5,P6,0,P8,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,P6,1,1,0,P10,P11,P12,P13,P14),board(P0,P1,P2,P3,P4,P5,P6,0,0,1,P10,P11,P12,P13,P14)).
move(board(P0,0,P2,P3,1,P5,P6,P7,1,P9,P10,P11,P12,P13,P14),board(P0,1,P2,P3,0,P5,P6,P7,0,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,0,1,1,P9,P10,P11,P12,P13,P14),board(P0,P1,P2,P3,P4,P5,1,0,0,P9,P10,P11,P12,P13,P14)).
move(board(P0,P1,0,P3,P4,1,P6,P7,P8,1,P10,P11,P12,P13,P14),board(P0,P1,1,P3,P4,0,P6,P7,P8,0,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,P6,0,1,1,P10,P11,P12,P13,P14),board(P0,P1,P2,P3,P4,P5,P6,1,0,0,P10,P11,P12,P13,P14)).
move(board(P0,P1,P2,0,P4,P5,1,P7,P8,P9,1,P11,P12,P13,P14),board(P0,P1,P2,1,P4,P5,0,P7,P8,P9,0,P11,P12,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,1,1,0,P13,P14),board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,0,0,1,P13,P14)).
move(board(P0,P1,P2,P3,0,P5,P6,1,P8,P9,P10,1,P12,P13,P14),board(P0,P1,P2,P3,1,P5,P6,0,P8,P9,P10,0,P12,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,1,1,0,P14),board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,0,0,1,P14)).
move(board(P0,P1,P2,0,P4,P5,P6,1,P8,P9,P10,P11,1,P13,P14),board(P0,P1,P2,1,P4,P5,P6,0,P8,P9,P10,P11,0,P13,P14)).
move(board(P0,P1,P2,P3,P4,0,P6,P7,1,P9,P10,P11,1,P13,P14),board(P0,P1,P2,P3,P4,1,P6,P7,0,P9,P10,P11,0,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,0,1,1,P13,P14),board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,1,0,0,P13,P14)).
move(board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,P11,1,1,0),board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,P11,0,0,1)).
move(board(P0,P1,P2,P3,0,P5,P6,P7,1,P9,P10,P11,P12,1,P14),board(P0,P1,P2,P3,1,P5,P6,P7,0,P9,P10,P11,P12,0,P14)).
move(board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,0,1,1,P14),board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,1,0,0,P14)).
move(board(P0,P1,P2,P3,P4,0,P6,P7,P8,1,P10,P11,P12,P13,1),board(P0,P1,P2,P3,P4,1,P6,P7,P8,0,P10,P11,P12,P13,0)).
move(board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,P11,0,1,1),board(P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,P11,1,0,0)).


/// initial_state(PegCount,Board)
/// Defines the initial board configuration and the number of pegs.
///
initial_state(14,board(0,1,1,1,1,1,1,1,1,1,1,1,1,1,1)).

/// final_state(PegCount,Board)
/// Defines the final board configuration and the number of pegs.
///
final_state(1,Board).

/// solve(+PegCount,+Boards,FinalPegCount,FinalBoards)
/// Determines the final peg count and list of boards given a current peg count and list of boards.
///
solve(PegCount,Boards,PegCount,Boards) :-
    ([B|Bs] = Boards),
    final_state(PegCount,B).
solve(PegCount,Boards,FinalPegCount,FinalBoards) :-
    next_move(PegCount,Boards,NextPegCount,NextBoards),
    solve(NextPegCount,NextBoards,FinalPegCount,FinalBoards).

/// next_move(+PegCount,+Boards,NextPegCount,NextBoards)
/// Determines the next peg count and list of boards given a current peg count and list of boards.
///
next_move(PegCount,[Board|Boards],NextPegCount,[NextBoard,Board|Boards]) :-
    move(Board,NextBoard),
    (NextPegCount := (PegCount - 1)).


/// Returns the final list of boards.
///
solve(FinalBoards) :-
    initial_state(InitialPegCount,InitialBoard),
    solve(InitialPegCount,[InitialBoard],FinalPegCount,FinalBoards).

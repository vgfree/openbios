\ 7.6 Client Program Debugging command group


\ 7.6.1    Registers display

: ctrace    ( -- )
  ;
  
: .registers    ( -- )
  ;

: .fregisters    ( -- )
  ;

\ to    ( param [old-name< >] -- )


\ 7.6.2    Program download and execute

struct ( saved-program-state )
  /n field >sps.entry
  /n field >sps.file-size
  /n field >sps.file-type
constant saved-program-state.size
create saved-program-state saved-program-state.size allot

variable state-valid
0 state-valid !

variable file-size

: !load-size file-size ! ;

: load-size file-size @ ;


\ File types identified by (init-program)

0  constant elf-boot
1  constant elf
2  constant bootinfo
3  constant xcoff
4  constant pe
5  constant aout
10 constant fcode
11 constant forth


: init-program    ( -- )
  \ Call down to the lower level for relocation etc.
  s" (init-program)" $find if
    execute
  else
    s" Unable to locate (init-program)!" type cr
  then
  ;

: (encode-bootpath) ( "{params}<cr>" -- bootpath-str bootpath-len)
  bl parse 2dup encode-string
  " /chosen" (find-dev) if
    " bootpath" rot (property)
  then
  linefeed parse encode-string
  " /chosen" (find-dev) if
    " bootargs" rot (property)
  then
;

: load    ( "{params}<cr>" -- )
  (encode-bootpath)
  open-dev ( ihandle )
  dup 0= if
    drop
    exit
  then
  dup >r
  " load-base" evaluate swap ( load-base ihandle )
  dup ihandle>phandle " load" rot find-method ( xt 0|1 )
  if swap call-package !load-size else cr ." Cannot find load for this package" 2drop then
  r> close-dev
  init-program
  ;

: dir ( "{paths}<cr>" -- )
  linefeed parse
  open-dev dup 0= if
    drop
    exit
  then
  dup
  " dir" rot ['] $call-method catch
  if
    3drop
    cr ." Cannot find dir for this package"
  then
  close-dev
;

: go    ( -- )
  state-valid @ not if exit then
  elf saved-program-state >sps.file-type @ = if
[IFDEF] CONFIG_PPC
    saved-program-state >sps.entry @ " (go)" evaluate
[ELSE]
    ." go is not yet implemented"
[THEN]
  else
    xcoff saved-program-state >sps.file-type @ = if
[IFDEF] CONFIG_PPC
      saved-program-state >sps.entry @ " (go)" evaluate
[ELSE]
      ." go is not yet implemented"
[THEN]
    else
      ." go is not yet implemented"
    then
  then
  ;


\ 7.6.3    Abort and resume

\ already defined !?
\ : go    ( -- )
\   ;

  
\ 7.6.4    Disassembler

: dis    ( addr -- )
  ;
  
: +dis    ( -- )
  ;

\ 7.6.5    Breakpoints
: .bp    ( -- )
  ;

: +bp    ( addr -- )
  ;

: -bp    ( addr -- )
  ;

: --bp    ( -- )
  ;

: bpoff    ( -- )
  ;

: step    ( -- )
  ;

: steps    ( n -- )
  ;

: hop    ( -- )
  ;

: hops    ( n -- )
  ;

\ already defined
\ : go    ( -- )
\   ;

: gos    ( n -- )
  ;

: till    ( addr -- )
  ;

: return    ( -- )
  ;

: .breakpoint    ( -- )
  ;

: .step    ( -- )
  ;

: .instruction    ( -- )
  ;


\ 7.6.6    Symbolic debugging
: .adr    ( addr -- )
  ;

: sym    ( "name< >" -- n )
  ;

: sym>value    ( addr len -- addr len false | n true )
  ;

: value>sym    ( n1 -- n1 false | n2 addr len true )
  ;

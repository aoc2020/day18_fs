module day18.Parser

open System

type Token =
    | T_Number of uint64
    | T_Plus
    | T_Mult 
    | T_StartParan 
    | T_EndParan 

type CToken =
    | C_Number of List<char>
    | C_Single of char 

let rec split (s:List<char>) : List<Token>  =
    let isDigit c = c >= '0' && c <= '9'
//    printfn "Tokenize: %A" s
//    let appendDigit (digit:char) (tail:List<char>) : List<CToken> = 
//        let rest = split tail
//        match rest with
//         | [] -> [C_Number ['d']]
//         | (C_Single _)::_ -> (C_Number [digit]) :: rest
//         | (C_Number n)::others -> (C_Number (digit::n)) :: others
         
    match s with
        | [] -> []
        | ' ' :: tail -> split tail
        | '(' :: tail -> T_StartParan :: split tail
        | ')' :: tail -> T_EndParan :: split tail
        | '+' :: tail -> T_Plus :: split tail
        | '*' :: tail -> T_Mult :: split tail
        | d :: tail when isDigit d -> (T_Number ((d |> uint64) - ('0' |> uint64))) :: split tail 
//        | d::tail when isDigit d -> appendDigit d tail
        
let tokensToString (tokens:List<Token>) : String =
    let toS (t:Token) =
        match t with
            | T_Number n -> sprintf "%d" n
            | T_StartParan -> sprintf "("
            | T_EndParan -> sprintf ")"
            | T_Plus -> sprintf "+"
            | T_Mult -> sprintf "*"
    tokens |> Seq.map toS |> String.Concat 
                                 
let tokenize (s:String) : List<Token> =
    let tokens = ( s.ToCharArray () |> Seq.toList |> split )
    in printfn "Split: %A" tokens 
    tokens 

type Expression =
    | Number of uint64
    | Plus of Expression * Expression
    | Mult of Expression * Expression

let rec parseRec (input:List<Token>) : Expression*(List<Token>) =
    match input with
        | T_Number n :: T_Plus :: tail ->
            let parsedTail = parseRec tail
            let expr2: Expression = fst parsedTail
            let rest : List<Token> = snd parsedTail 
            in Plus ((Number n),expr2), rest
        | T_Number n :: T_Mult :: tail ->
            let parsedTail = parseRec tail
            let expr2: Expression = fst parsedTail
            let rest : List<Token> = snd parsedTail 
            in Mult ((Number n),expr2), rest
        | T_Number n :: T_EndParan :: rest -> Number (n |> uint64), rest
        | [T_Number n] -> Number (n |> uint64), [] 
        | T_StartParan :: tail -> parseRec tail
        
let rec eval1 (greedy:bool) (tokens:List<Token>) : List<Token> =
//    printfn "EVAL %A %A" greedy (tokensToString tokens)   
    match tokens with
    | [T_Number n] -> [T_Number n]
    | T_Number a :: (op :: (T_StartParan :: rest)) -> eval1 false ((T_Number a)::(op::(eval1 false (T_StartParan :: rest))))
    | T_Number a :: (T_EndParan :: _) -> tokens 
    | T_Number a :: (T_Plus :: (T_Number b :: tail)) -> eval1 greedy ((T_Number (a+b)) :: tail)
    | T_Number a :: (T_Mult :: (T_Number b :: tail)) -> eval1 greedy ((T_Number (a*b)) :: tail)  
    | T_StartParan :: (T_Number n) :: T_EndParan :: rest ->
        let c = (T_Number n :: rest) in if greedy then eval1 true c else c
    | T_StartParan :: tail -> eval1 greedy (T_StartParan :: (eval1 greedy tail)) 

let run1 (input:String) : uint64 =
    let chars = input.ToCharArray () |> Seq.toList
    let tokens = tokenize input
    let result = eval1 true tokens
//    printfn "Result: %A" result
    match List.head result with
    | T_Number n -> n
    
let rec eval2 (greedy:bool) (tokens:List<Token>) : List<Token> =
//    printfn "EVAL2 %A %A" greedy (tokensToString tokens)   
    match tokens with
    | [T_Number n] -> [T_Number n]
    | T_Number a :: (op :: (T_StartParan :: rest)) -> eval2 false ((T_Number a)::(op::(eval2 false (T_StartParan :: rest))))
    | T_Number a :: (T_EndParan :: _) -> tokens 
    | T_Number a :: (T_Plus :: (T_Number b :: tail)) -> eval2 greedy ((T_Number (a+b)) :: tail)
    | T_Number a :: (T_Mult :: (T_Number b :: tail)) ->
        let tailres = eval2 false (T_Number b :: tail)
        match tailres with
        | (T_Number c) :: rest -> eval2 greedy ((T_Number (a*c)) :: rest) 
    | T_StartParan :: (T_Number n) :: T_EndParan :: rest ->
        let c = (T_Number n :: rest) in if greedy then eval2 true c else c
    | T_StartParan :: tail -> eval2 greedy (T_StartParan :: (eval2 greedy tail)) 

let run2 (input:String) : uint64 =
    let chars = input.ToCharArray () |> Seq.toList
    let tokens = tokenize input
    let result = eval2 true tokens
//    printfn "Result: %A" result
    match List.head result with
    | T_Number n -> n  

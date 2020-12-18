// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open day18.BasicTypes
open day18.Utilities
open day18.Parser 
open day18.IO 

    let input = "/Users/xeno/projects/aoc2020/day18_fs/input.txt"
    let input2 = "/Users/xeno/projects/aoc2020/day18_fs/input2.txt"


let readInput (filePath:String) : String[] =
    let input = readFile filePath |> Seq.toArray 
    printfn "input: %A" input
    input 
    
    
[<EntryPoint>]
let main argv =
      let lines = readInput input |> Seq.toArray
      let results = lines |> Array.map  run
      let result = results |> Seq.sum
//    printfn "%A" (run "2 * 3 + (4 * 5)") // 26 
//    printfn "%A" (run "5 + (8 * 3 + 9 + 3 * 4 * 3)") // becomes 437.
//    printfn "%A" (run "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))") // becomes 12240.
//      printfn "%A" (run "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2") // becomes 13632.
      printfn "Result! %A" result    
      0 // return an integer exit code
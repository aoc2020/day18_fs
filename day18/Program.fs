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
//      let results = lines |> Array.map run1
//      let result = results |> Seq.sum
      printfn "%A" (run2 "1 + 2 * 3 + 4 * 5 + 6")
      
      let results = lines |> Array.map run2
      let result = results |> Seq.sum
      printfn "Result 2! %A" result

      0 // return an integer exit code
      
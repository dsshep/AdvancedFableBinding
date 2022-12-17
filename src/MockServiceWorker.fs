module MockServiceWorker 
    open System
    open Fable.Core
    
    type Request = 
        interface end

    type ResponseResolver = 
        interface end

    type ResponseTransformer = 
        interface end

    type Context = 
        abstract member status : code : int -> ResponseTransformer
        abstract member json<'T> : obj : 'T -> ResponseTransformer

    type RestHandler = 
        interface end
    
    type Result = delegate of [<ParamArray>] transformers : ResponseTransformer[] -> ResponseResolver

    type Rest =
        abstract member post: 
            path : string * 
            fn: (Request -> Result -> Context -> ResponseResolver)
                -> RestHandler

        abstract member get: 
            path : string * 
            fn: (Request -> Result -> Context -> ResponseResolver)
                -> RestHandler

    type Worker = 
        abstract member start : unit -> unit

    [<Import("setupWorker", from="msw")>]
    [<Emit("setupWorker(...$1)")>]
    let setupWorker (handlers : RestHandler[]) : Worker = jsNative

    [<Import("rest", from="msw")>]
    let rest : Rest = jsNative
    
    let createHandlers() =
        let handler = 
            rest.get(
                "/api/user",
                fun req res ctx -> 
                    res.Invoke
                        (ctx.status(200),
                         ctx.json({| Username = "Admin" |}))
                    )
        [| handler |]

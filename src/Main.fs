module Main

open Feliz
open App
open Browser.Dom

#if DEBUG
open MockServiceWorker
let handlers = createHandlers()
let worker = setupWorker handlers
worker.start()
#endif

ReactDOM.render(
    Components.MswComponent(),
    document.getElementById "app"
)
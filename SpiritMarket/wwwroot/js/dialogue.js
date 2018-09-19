$(document).ready(function(){
    function getSyncScriptParams() {
        var scripts = document.getElementsByTagName('script');
        var lastScript = scripts[scripts.length-1];
        var scriptName = lastScript;
        return scriptName.getAttribute('data-textfile');
    }
    var textfile = getSyncScriptParams();
    var alertdata;
    $.getJSON(textfile, function(data){
        var DialogueList = data["Dialogues"];
        var Header = data["SceneTitle"] || "Event";
        alertdata = Header;

        $("#randomevent").html(function(){
            var toreturn = "<ul>";
            for(var dialogue of data["Dialogues"]){
                toreturn += "<li>" + dialogue + "</li>";
            }
            return toreturn + '</ul><button onclick="testfunction()" class="btn btn-success sendalert">Spawn alert</button>';
        });
    });

    $("body").on("click", ".sendalert", function(){
        alert(alertdata);
    })

});

{/* <div class="card border-success mb-3 mt-3 ml-auto mr-auto" style="max-width: 75%;">
    <div class="card-header">Header</div>
        <div class="card-body text-success">
        <h5 class="card-title">Success card title</h5>
        <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
    </div>
</div>

 */}

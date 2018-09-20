$(document).ready(function(){
    function getSyncScriptParams() {
        var scripts = document.getElementsByTagName('script');
        var lastScript = scripts[scripts.length-1];
        var scriptName = lastScript;
        return scriptName.getAttribute('data-textfile');
    }
    var textfile = getSyncScriptParams();


    var DialogueList;
    var DialogueIndex;
    $.getJSON(textfile, function(data){

        

        DialogueList = data["Dialogues"];
        DialogueIndex = 0;
        var Header = data["SceneTitle"] || "Event";

        $("#randomevent").html(function(){
            var toreturn =  '<div class="card border-success mb-3 mt-3 ml-auto mr-auto" style="max-width: 75%;">' + 
                                '<div class="card-header">' + Header + '</div>' + 
                                '<div class="card-body text-success text-center">' + 
                                    '<p class="card-text dialoguecontent">' + DialogueList[DialogueIndex] + '</p>' + 
                                    '<div class="dialoguebuttons btn-group m-2 ml-auto mr-auto" role="group" style="min-width:40%;">' + 
                                        '<button class="btn btn-danger backdialogue" style="min-width:50%;">Back</button>' +
                                        '<button class="btn btn-success nextdialogue" style="min-width:50%;">Next</button>' + 
                                    '</div>' + 
                                '</div>' + 
                            '</div>';
            return toreturn;
        });
        updateDialogueButtons();
    });

    $("body").on("click", ".nextdialogue", function(){
        DialogueIndex += 1;
        $(".dialoguecontent").html(DialogueList[DialogueIndex]);
        updateDialogueButtons();
    })

    $("body").on("click", ".backdialogue", function(){
        DialogueIndex -= 1;
        $(".dialoguecontent").html(DialogueList[DialogueIndex]);
        updateDialogueButtons();
    })

    function updateDialogueButtons(){
        if(DialogueIndex != 0){
            $(".backdialogue").attr("disabled", false);
        }
        else{
            $(".backdialogue").attr("disabled", true);
        }
        if(DialogueIndex < DialogueList.length - 1){
            $(".nextdialogue").attr("disabled", false);
        }
        else{
            $(".nextdialogue").attr("disabled", true);
        }
    }

});



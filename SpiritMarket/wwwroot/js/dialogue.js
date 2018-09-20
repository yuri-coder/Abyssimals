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
    var CloseTrigger;

    $.getJSON(textfile, function(data){

        

        DialogueList = data["Dialogues"];
        DialogueIndex = 0;
        var Header = data["SceneTitle"] || "Event";
        CloseTrigger = data["CloseTrigger"];

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

    function defaultNextDialogue() {
        DialogueIndex += 1;

        $(".dialoguecontent").fadeOut(function(){
            $(this).html(DialogueList[DialogueIndex]);
        }).fadeIn();

        updateDialogueButtons();
    }

    function defaultBackDialogue() {
        DialogueIndex -= 1;

        $(".dialoguecontent").fadeOut(function(){
            $(this).html(DialogueList[DialogueIndex]);
        }).fadeIn();

        updateDialogueButtons();
    }

    $("body").on("click", ".nextdialogue", function(){
        defaultNextDialogue();
    })

    $("body").on("click", ".backdialogue", function(){
        defaultBackDialogue()
    })

    function updateDialogueButtons(){
        if(DialogueIndex != 0){
            $(".backdialogue").attr("disabled", false);
        }
        else{
            $(".backdialogue").attr("disabled", true);
        }
        if(DialogueIndex < DialogueList.length - 1){
            switch(CloseTrigger){
                case "LastDialogue":
                    if($(".nextdialogue").hasClass("btn-warning")){
                        $(".nextdialogue").fadeOut(function(){
                            $(this).text("Next").addClass("btn-success").removeClass("btn-warning text-light");
                        }).fadeIn();
                        $("body").off("click", ".nextdialogue");
                        $("body").on("click", ".nextdialogue", function(){
                            defaultNextDialogue();
                        });
                    }
                    break;
                default:
                    $(".nextdialogue").attr("disabled", false);    
            }
        }
        else{
            switch(CloseTrigger){
                case "LastDialogue":
                    $(".nextdialogue").fadeOut(function(){
                        $(this).text("Finish").addClass("btn-warning text-light").removeClass("btn-success");
                    }).fadeIn();
                        //$(".nextdialogue").text("Finish").addClass("btn-warning text-light").removeClass("btn-success");
                    $("body").on("click", ".nextdialogue", function(){
                        $("#randomevent").fadeOut(function(){
                            $(this).remove();
                        });
                    });
                    break;
                default:
                    $(".nextdialogue").attr("disabled", true);
            }
        }
    }

});



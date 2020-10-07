// @ts-ignore
declare var Postmonger: any;

var connection = new Postmonger.Session();

let inputConfiguration:any;
console.log("iframe location " + document.location.origin)

let host = document.location.origin

connection.trigger('ready');

connection.on('initActivity', function (payload: any) {
    let activity = payload;

    const hasInArguments = Boolean(
        activity.arguments &&
        activity.arguments.execute &&
        activity.arguments.execute.inArguments &&
        activity.arguments.execute.inArguments.length > 0
    );

    const inArguments = hasInArguments ? activity.arguments.execute.inArguments : [];
    inputConfiguration = payload;
    console.log(inputConfiguration)
});

connection.on('clickedNext', function () {
    let planStateElement = <HTMLInputElement>document.getElementById("plan-state-selector");
    let planState = planStateElement.value;

    let userExitFlagElement = <HTMLInputElement>document.getElementById("user-exit-flag-selector");
    let userExit = userExitFlagElement.checked;

    let configuration = {
        "name": "",
        "id": null,
        "key": "REST-1",
        "arguments": {
            "execute": {
                "inArguments": [
                    { contactKey: "{{Contact.Key}}", planState, userExit },
                ],
                "url": inputConfiguration.arguments.execute.url,
                "verb": "POST",
                "header": "",
                "format": "json",
                "useJwt": true,
                // "customerKey": "<EXTERNAL KEY OF THE SALT KEY FOR THE JWT>",
                "timeout": 90000,
                "retryCount": 5,
                "retryDelay": 100
            }
        },
        "configurationArguments": {
            "publish": {
                "url": inputConfiguration.configurationArguments.publish.url
            }
        },
        "metaData": {
            "icon": inputConfiguration.metaData.icon,
            "category": "message",
            "iconSmall": null,
            "statsContactIcon": null,
            "original_icon": "sitecore.png",
            "isConfigured": true
        },
        "editable": true,
        "outcomes": [
            {
                "key": "3b7498c9-b1d2-4ae1-b7ff-5a31d6ac642c",
                "next": "WAITBYDURATION-1",
                "arguments": {},
                "metaData": {
                    "invalid": false
                }
            }
        ],
        // "edit": {
        //     "url": inputConfiguration.edit.url,
        //     "height": 200,
        //     "width": 500
        // }
    }

    console.log(configuration)
    connection.trigger('updateActivity', configuration);
})
import {onready} from './init';
import { ClimbClient } from "../gen/climbClient";

onready(async () => {
    var openButton = document.getElementById("challenge-button");
    if (openButton != null) {
        openButton.onclick = () => toggleChallengeModal(true);
    }

    var closeButton = document.getElementById("challenge-modal-close");
    if (closeButton != null) {
        closeButton.onclick = () => toggleChallengeModal(false);
    }

    var requestButtons = document.getElementsByClassName("request-button");
    for (let i = 0; i < requestButtons.length; i++) {
        const button = <HTMLButtonElement>requestButtons[i];
        const requester = button.getAttribute("data-requester");
        const challenged = button.getAttribute("data-challenged");
        if (requester != null && challenged != null) {
            button.onclick = () => sendRequest(parseInt(requester), parseInt(challenged));
        }
    }
});

function toggleChallengeModal(open: Boolean)
{
    const challengeModal = document.getElementById("challenge-modal");
    if (challengeModal == null) {
        return;
    }

    challengeModal.hidden = !open;
}

function sendRequest(requesterId: number, challengedId: number) {
    const setApi = new ClimbClient.SetApi();
    setApi.challengeUser(requesterId, challengedId)
        .then((setRequest: ClimbClient.SetRequest): void => {
            console.log(setRequest);
            window.location.reload();
        })
        .catch((reason: any) => alert(`Could not challenge user.\n${reason}`));
}
import { ClimbClient } from "../gen/climbClient";

function openChallengeModal() {
    toggleChallengeModal(true);
}

function closeChallengeModal() {
    toggleChallengeModal(false);
}

function toggleChallengeModal(open: Boolean)
{
    const challengeModal = document.getElementById("challenge-modal");
    if (challengeModal == null) {
        return;
    }

    challengeModal.hidden = !open;
}

function sendRequest(requesterId: number, challengedId: number) {
    //alert(requesterId + "vs" + challengedId);
    const setApi = new ClimbClient.SetApi();
    setApi.challengeUser(requesterId, challengedId)
        .then((setRequest: ClimbClient.SetRequest): void => {
            console.log(setRequest);
            window.location.reload();
        })
        .catch((reason: any) => alert(`Could not challenge user.\n${reason}`));
}
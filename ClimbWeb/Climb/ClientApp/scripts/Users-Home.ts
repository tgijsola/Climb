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
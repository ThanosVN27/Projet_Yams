document.getElementById('load-game').addEventListener('click', async function () {
    const gameId = document.getElementById('game-id').value;

    if (!gameId) {
        alert('Veuillez entrer un ID de jeu');
        return;
    }

    const roundsContainer = document.getElementById('rounds-container');
    roundsContainer.innerHTML = ''; // Réinitialiser le conteneur

    let player1Pseudo = 'Joueur 1';
    let player2Pseudo = 'Joueur 2';

    // Charger les paramètres de la partie
    async function chargerParametres(gameId) {
        const url = `http://yams.iutrs.unistra.fr:3000/api/games/${gameId}/parameters`;
        const response = await fetch(url);
        const data = await response.json();

        const gameCodeElement = document.getElementById('game-code');
        const gameDateElement = document.getElementById('game-date');

        if (gameCodeElement) gameCodeElement.textContent = data.code || 'Code indisponible';
        if (gameDateElement) gameDateElement.textContent = data.date || 'Date indisponible';
    }

    // Charger les joueurs
    async function chargerJoueurs(gameId) {
        const url = `http://yams.iutrs.unistra.fr:3000/api/games/${gameId}/players`;
        const response = await fetch(url);
        const data = await response.json();

        player1Pseudo = data[0]?.pseudo || 'Joueur 1';
        player2Pseudo = data[1]?.pseudo || 'Joueur 2';

        const player1Element = document.getElementById('pseudo-joueur1');
        const player2Element = document.getElementById('pseudo-joueur2');

        // Mise à jour des pseudos dans la section des joueurs
        if (player1Element) player1Element.textContent = player1Pseudo;
        if (player2Element) player2Element.textContent = player2Pseudo;
    }

    // Générer les rounds dynamiquement
    function genererRounds() {
        for (let i = 1; i <= 13; i++) {
            const roundDiv = document.createElement('div');
            roundDiv.classList.add('round');
            roundDiv.id = `round-${i}`;

            roundDiv.innerHTML = `
                <h3>Tour ${i}</h3>
                <div class="player-results">
                    <div class="player-result j1">
                        <h4>${player1Pseudo}</h4> <!-- Afficher le pseudo du joueur 1 -->
                        <p><strong>Challenge:</strong> <span id="challenge-joueur1-round${i}">...</span></p>
                        <p><strong>Score:</strong> <span id="score-joueur1-round${i}">0</span></p>
                        <div class="joueur1-des-round${i} dice-container"></div>
                    </div>
                    <div class="player-result j2">
                        <h4>${player2Pseudo}</h4> <!-- Afficher le pseudo du joueur 2 -->
                        <p><strong>Challenge:</strong> <span id="challenge-joueur2-round${i}">...</span></p>
                        <p><strong>Score:</strong> <span id="score-joueur2-round${i}">0</span></p>
                        <div class="joueur2-des-round${i} dice-container"></div>
                    </div>
                </div>
            `;

            roundsContainer.appendChild(roundDiv);
        }
    }

    // Charger les rounds
    async function chargerRounds(gameId) {
        for (let i = 1; i <= 13; i++) {
            const url = `http://yams.iutrs.unistra.fr:3000/api/games/${gameId}/rounds/${i}`;
            const response = await fetch(url);
            const data = await response.json();
            mettreAJourDonneesRound(data, i);
        }
    }

    // Mettre à jour les données pour chaque round
    function mettreAJourDonneesRound(roundData, roundIndex) {
        const challengeJ1 = document.getElementById(`challenge-joueur1-round${roundIndex}`);
        const challengeJ2 = document.getElementById(`challenge-joueur2-round${roundIndex}`);
        const scoreJ1 = document.getElementById(`score-joueur1-round${roundIndex}`);
        const scoreJ2 = document.getElementById(`score-joueur2-round${roundIndex}`);
        const diceJ1 = document.querySelector(`.joueur1-des-round${roundIndex}`);
        const diceJ2 = document.querySelector(`.joueur2-des-round${roundIndex}`);

        const player1Data = roundData.results.find(r => r.id_player === 1);
        const player2Data = roundData.results.find(r => r.id_player === 2);

        if (player1Data) {
            if (challengeJ1) challengeJ1.textContent = player1Data.challenge || '...';
            if (scoreJ1) scoreJ1.textContent = player1Data.score || '0';
            if (diceJ1) diceJ1.innerHTML = player1Data.dice.map(d => `<img src="img/Dice${d}.png" alt="Dé ${d}" class="dice-image">`).join('');  
        }

        if (player2Data) {
            if (challengeJ2) challengeJ2.textContent = player2Data.challenge || '...';
            if (scoreJ2) scoreJ2.textContent = player2Data.score || '0';
            if (diceJ2) diceJ2.innerHTML = player2Data.dice.map(d => `<img src="img/Dice${d}.png" alt="Dé ${d}" class="dice-image">`).join('');
        }
    }

    // Charger le résultat final
    async function chargerResultatFinal(gameId) {
        const url = `http://yams.iutrs.unistra.fr:3000/api/games/${gameId}/final-result`;
        const response = await fetch(url);
        const data = await response.json();
        const scoreJ1Element = document.getElementById('scoreJoueur1');
        const scoreJ2Element = document.getElementById('scoreJoueur2');

        if (scoreJ1Element) scoreJ1Element.textContent = data[0]?.score || '0';
        if (scoreJ2Element) scoreJ2Element.textContent = data[1]?.score || '0';
    }

    // Charger toutes les données
    await chargerParametres(gameId);
    await chargerJoueurs(gameId);
    genererRounds();
    await chargerRounds(gameId);
    await chargerResultatFinal(gameId);
});

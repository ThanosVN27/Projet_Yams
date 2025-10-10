document.getElementById('load-game').addEventListener('click', function () {
    const gameId = document.getElementById('game-id').value;

    if (!gameId) {
        alert('Veuillez entrer un ID de jeu');
        return;
    }

    const url = `http://yams.iutrs.unistra.fr:3000/api/games`;
    const gameUrl = `${url}/${gameId}/parameters`;
    const playerUrl = `${url}/${gameId}/players`;
    const finalResultUrl = `${url}/${gameId}/final-result`;

    let currentRoundIndex = 1;
    const totalRounds = 13;

    // Charger les paramètres du jeu
    fetchData(gameUrl)
        .then(gameData => {
            const { code, date } = gameData;
            document.getElementById('game-code').textContent = code;
            document.getElementById('game-date').textContent = date;

            // Charger les informations des joueurs
            return fetchData(playerUrl);
        })
        .then(players => {
            const player1 = players[0];
            const player2 = players[1];

            document.getElementById('pseudo-joueur1').textContent = player1.pseudo;
            document.getElementById('pseudo-joueur2').textContent = player2.pseudo;

            // Charger le premier round
            loadRound(gameId, currentRoundIndex, player1.id, player2.id);

            document.getElementById('previous-round').addEventListener('click', function () {
                if (currentRoundIndex > 1) {
                    currentRoundIndex--;
                    loadRound(gameId, currentRoundIndex, player1.id, player2.id);
                }
            });

            document.getElementById('next-round').addEventListener('click', function () {
                if (currentRoundIndex < totalRounds) {
                    currentRoundIndex++;
                    loadRound(gameId, currentRoundIndex, player1.id, player2.id);
                }
            });
        })
        .catch(error => {
            handleError(error);
        });

    function loadRound(gameId, roundIndex, player1Id, player2Id) {
        const roundUrl = `http://yams.iutrs.unistra.fr:3000/api/games/${gameId}/rounds/${roundIndex}`;

        fetchData(roundUrl)
            .then(round => {
                const player1Data = round.results.find(result => result.id_player === player1Id);
                const player2Data = round.results.find(result => result.id_player === player2Id);

                // Mettre à jour l'affichage des dés
                updateDiceDisplay(player1Data.dice, 'joueur1');
                updateDiceDisplay(player2Data.dice, 'joueur2');

                // Mettre à jour les autres informations
                document.getElementById('challenge-joueur1').textContent = `Challenge : ${player1Data.challenge}`;
                document.getElementById('score-joueur1').textContent = `Point : ${player1Data.score}`;
                document.getElementById('challenge-joueur2').textContent = `Challenge : ${player2Data.challenge}`;
                document.getElementById('score-joueur2').textContent = `Point : ${player2Data.score}`;

                // Mise à jour du titre du round
                document.querySelector('.Jeu h5').textContent = `-----------Tour ${roundIndex} sur 13-----------`;

                // Si on atteint le dernier round, on charge les scores finaux
                if (roundIndex === totalRounds) {
                    updateTotalScores(gameId);
                }
            })
            .catch(error => {
                handleError(error);
            });
    }

    function updateTotalScores(gameId) {
        fetchData(finalResultUrl)
            .then(finalResult => {
                const player1Final = finalResult.find(result => result.id_player === 1);
                const player2Final = finalResult.find(result => result.id_player === 2);

                document.getElementById('scoreJoueur1').textContent = player1Final.score;
                document.getElementById('scoreJoueur2').textContent = player2Final.score;
            })
            .catch(error => {
                handleError(error);
            });
    }

    // Fonction générique pour récupérer les données
    function fetchData(url) {
        return fetch(url)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Erreur HTTP! Statut: ${response.status}`);
                }
                return response.json();
            });
    }

    // Fonction pour mettre à jour l'affichage des dés
    function updateDiceDisplay(diceValues, playerId) {
        const diceContainer = document.querySelector(`.${playerId}-des`);
        if (diceContainer) {
            diceContainer.innerHTML = diceValues.map(value => {
                return `<img src="img/Dice${value}.png" alt="Dé ${value}" class="dice-image">`;
            }).join('');
        } else {
            console.error(`Conteneur de dés pour ${playerId} introuvable.`);
        }
    }

    // Gestion des erreurs
    function handleError(error) {
        console.error('Erreur :', error);
        document.getElementById('game-code').textContent = 'Erreur lors du chargement';
        document.getElementById('game-date').textContent = 'Erreur lors du chargement';
        document.getElementById('pseudo-joueur1').textContent = 'Erreur lors du chargement';
        document.getElementById('pseudo-joueur2').textContent = 'Erreur lors du chargement';
        document.getElementById('scoreJoueur1').textContent = '0';
        document.getElementById('scoreJoueur2').textContent = '0';
    }
});

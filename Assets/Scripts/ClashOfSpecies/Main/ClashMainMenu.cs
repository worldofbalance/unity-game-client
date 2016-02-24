using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class ClashMainMenu : MonoBehaviour {
    private ClashGameManager manager;

	public VerticalLayoutGroup playerListGroup;
	public List<Player> playerList = new List<Player>();
	public Transform contentPanel;
    public GameObject playerItemPrefab;
	public Button attackBtn;

    private Player selectedPlayer = null;
    private ToggleGroup toggleGroup;

	void Awake() {
        manager = GameObject.Find("MainObject").GetComponent<ClashGameManager>();
    }

	void Start() {
		if (manager.currentPlayer.credits < 10) {
			attackBtn.interactable = false;
		}

		contentPanel.Find("Credit").GetComponent<Text>().text = "You have " + manager.currentPlayer.credits + " credits.";

        NetworkManager.Send(ClashPlayerListProtocol.Prepare(), (res) => {
            var response = res as ResponseClashPlayerList;

            foreach (var pair in response.players) {
                Player player = new Player(pair.Key);
                player.name = pair.Value;
                playerList.Add(player);

                var item = Instantiate(playerItemPrefab) as GameObject;
                item.transform.SetParent(playerListGroup.transform);
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 0.0f);
                item.transform.localScale = Vector3.one;

                item.GetComponentInChildren<Text>().text = player.name;

                var toggle = item.GetComponentInChildren<Toggle>().group = playerListGroup.GetComponent<ToggleGroup>();
                item.GetComponentInChildren<Toggle>().onValueChanged.AddListener((val) => {
                    contentPanel.Find("Message").GetComponent<Text>().enabled = !val;
                    if (val) {
                        selectedPlayer = player;
                        manager.currentTarget = new ClashDefenseConfig();
                        NetworkManager.Send(ClashPlayerViewProtocol.Prepare(player.GetID()), (resView) => {
                            var responseView = resView as ResponseClashPlayerView;
//                            Debug.Log(responseView.terrain);
                            contentPanel.GetComponent<RawImage>().texture = Resources.Load("Images/ClashOfSpecies/" + responseView.terrain) as Texture;
                            manager.currentTarget.owner = player;
                            manager.currentTarget.terrain = responseView.terrain;
                            manager.currentTarget.layout = responseView.layout.Select(x => {
                                var species = manager.availableSpecies.Single(s => s.id == x.Key);
                                var positions = x.Value;
                                return new { 
                                    species,
                                    positions
                                };
                            }).ToDictionary(p => p.species, p => p.positions);
                        });
                    } else {
                        selectedPlayer = null;
                        contentPanel.GetComponent<RawImage>().texture = null;
                    }
                });
            }
        });
    }

	void Update() {}

    public void ReturnToLobby() {
         Game.LoadScene("World");
    }

    public void EditDefense() {
        Game.LoadScene("ClashDefenseShop");
    }

    public void Attack() {
        if (manager.currentTarget != null) {
            Game.LoadScene("ClashAttackShop");
        }
    }
}

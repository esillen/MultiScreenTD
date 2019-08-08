using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour {

    private static RewardSystem singleton;
    public NetworkEconomySystem theEconomySystem;

    private void Start() {
        singleton = this;
    }

	public static int getReward(int level) {
        int reward = 1;
        singleton.theEconomySystem.currencyEvent(reward);
        return reward;
    }
}

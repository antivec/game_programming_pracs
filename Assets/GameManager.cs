using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField]
    BgScroll m_bgScroll;
    public enum GameState
    {
        Normal = 0,
        Invincible
    }
    GameState m_state;
    bool isSet;
    float m_InvincibleMagPower = 5.0f;
    public  void SetGameState(GameState state)
    {
        if (state == m_state)
            return;
        m_state = state;
        isSet = true;
    }
    private void SetInvincibleMode()
    {
        m_bgScroll.m_speed *= m_InvincibleMagPower;
        MonsterManager.Instance.SetMonsterSpeedInvincible(m_InvincibleMagPower);
        Player.Instance.SetInvincible();
        MonsterManager.Instance.ResetCreateMonster();
        isSet = false;
    }
    private void SetNormalMode()
    {
        m_bgScroll.m_speed /= m_InvincibleMagPower;
        MonsterManager.Instance.SetMonsterSpeedNormal(m_InvincibleMagPower);
        MonsterManager.Instance.ResetCreateMonster();
        isSet = false;
    }
    protected override void OnStart()
    {
        base.OnStart();
        m_state = GameState.Normal;
        isSet = false;
    }

    // Update is called once per frame
    void Update () {
        if (isSet)
        {
            switch (m_state)
            {
                case GameState.Normal:
                    SetNormalMode();
                    break;
                case GameState.Invincible:
                    SetInvincibleMode();
                    break;
            }
        }
	}
}

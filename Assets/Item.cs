using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public enum ItemType
    {
        Coin = 0,
        Gem_Red,
        Gem_Green,
        Gem_Blue,
        Invincible,
        Magnet
        
    }
    [SerializeField]
    SpriteRenderer m_renderer;
    Rigidbody2D m_rigidbody;
    float m_power = 1.3f;

    public ItemType m_type { get; set; }
	// Use this for initialization
	void Awake () {
        m_rigidbody = GetComponent<Rigidbody2D>();        
    }
    public void SetItem(ItemType type)
    {
        m_rigidbody.angularVelocity = 0f;
        m_rigidbody.velocity = Vector2.zero;
        m_rigidbody.transform.rotation = Quaternion.identity;
        m_rigidbody.isKinematic = false;        
        float axis_x = (Random.Range(0, 2) % 2) == 0 ? -0.1f : 0.1f;        
        m_rigidbody.AddForce(new Vector2(axis_x, 1f) * m_power, ForceMode2D.Impulse);
        if (type >= ItemType.Gem_Red && type <= ItemType.Gem_Blue)
            m_rigidbody.AddTorque(axis_x < 0 ? -1 : 1 * m_power, ForceMode2D.Impulse);
        m_type = type;
        m_renderer.sprite = ItemManager.Instance.GetItemSprite(type);
    }
    private void PlayItemGetSound()
    {
        if (m_type == ItemType.Coin)
            SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Get_Coin);
        else if (m_type == ItemType.Gem_Red || m_type == ItemType.Gem_Green || m_type == ItemType.Gem_Blue)
            SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Get_Gem);
        else if (m_type == ItemType.Invincible)
            SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Get_Invincible);
        else if (m_type == ItemType.Magnet)
            SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Get_Item);
    }
    private void SetItemEffect()
    {
        switch(m_type)
        {
            case ItemType.Invincible:
                GameManager.Instance.SetGameState(GameManager.GameState.Invincible);                
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ItemManager.Instance.RemoveItem(this);
            PlayItemGetSound();
            SetItemEffect();
        }
        if (collision.tag == "wall_bottom")
        {
            ItemManager.Instance.RemoveItem(this);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}

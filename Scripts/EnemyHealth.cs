using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public EnemyStats enemyStats;
    public Animator animator;

    public int currentHP;
    public Image hpBarForeground;
    public RectTransform loot;
    public TextMeshProUGUI lootText;

    private Coroutine takeDamageEffectCoroutine;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Color originalColor;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();

        currentHP = enemyStats.hp;
        UpdateHealthBar();
        loot.gameObject.SetActive(false);
        lootText.text = "+" + enemyStats.fleeingSouls;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        // decrease hp
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            animator.SetBool("isDie", true);
            StartCoroutine(FleeingSoul());
        }
        UpdateHealthBar();

        // apply take damage effect
        if (takeDamageEffectCoroutine != null)
        {
            StopCoroutine(takeDamageEffectCoroutine);
        }

        takeDamageEffectCoroutine = StartCoroutine(ChangeColorOnDamage());
    }

    private IEnumerator ChangeColorOnDamage()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        spriteRenderer.color = originalColor;
    }

    private void UpdateHealthBar()
    {
        hpBarForeground.fillAmount = (float)currentHP / enemyStats.hp;
    }

    public IEnumerator FleeingSoul()
    {
        loot.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Vector2 startPosition = loot.anchoredPosition;
        Vector2 endPosition = loot.anchoredPosition + new Vector2(0, 2);

        while (elapsedTime < 2)
        {
            loot.anchoredPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Die()
    {
        float randomValue = Random.value * 100;
        int bonus;
        if (randomValue < 30) // 30% coin += 0
            bonus = 0;
        else if (randomValue < 50) // 20% coin += 1
            bonus = 1;
        else if (randomValue < 70) // 20% coin += 2
            bonus = 2;
        else if (randomValue < 85) // 15% coin += 3
            bonus = 3;
        else if (randomValue < 95) // 10% coin += 5
            bonus = 5;
        else if (randomValue < 99) // 4% coin += 10
            bonus = 10;
        else // 1% coin += 50
            bonus = 50;

        BaseManagement.Instance.coins += bonus;
        BaseManagement.Instance.souls += enemyStats.fleeingSouls;
        Destroy(gameObject);
    }
}

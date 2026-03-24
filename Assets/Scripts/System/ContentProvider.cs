using System.Collections.Generic;
using UnityEngine;

public class ContentProvider : MonoBehaviour
{
     private DataConfigOS dataConfig;  // kéo asset GameConfig vào đây

     private List<TopicDataOS> activeTopics = new List<TopicDataOS>();

    public void SetActiveTopics(List<Gamephase> currentPhases)
    {
        activeTopics.Clear();

        // Duyệt qua tất cả các Gamephase trong List
        if (currentPhases != null && currentPhases.Count > 0)
        {
            foreach (Gamephase phase in currentPhases)
            {
                if (phase != null && phase.selectedTopic != null && phase.selectedTopic.Count > 0)
                {
                    activeTopics.AddRange(phase.selectedTopic); // Gộp tất cả topic lại
                }
            }
        }

        // Fallback: Nếu duyệt xong mà vẫn không có topic nào, lấy toàn bộ từ dataconfig
        if (activeTopics.Count == 0)
        {
            if (dataConfig != null && dataConfig.allTopics != null)
                activeTopics.AddRange(dataConfig.allTopics);
        }

        if (activeTopics.Count == 0)
            Debug.LogError("Không có topic nào khả dụng!");
        else
        Debug.Log($"✅ Active topics: {activeTopics.Count}");
    }
    public string GetRandomContent()
    {
        if (activeTopics.Count == 0) return "ERROR_NO_TOPIC";

        int rand = Random.Range(0, activeTopics.Count);
        TopicDataOS topic = activeTopics[rand];
        return topic.GetRandomContent();
    }
}

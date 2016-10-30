package foundation.softwaredesign.orbi.model.game.gameobject;

import javax.xml.bind.annotation.XmlTransient;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObjectType {

    private Long id;
    private String prefab;
    private Integer rarity;
    @XmlTransient
    private Integer ordering;
    @XmlTransient
    private Integer spawnAmount;
    private Boolean supportsUserText;
    @XmlTransient
    private Long categoryId;
    private GameObjectTypeCategory category;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getPrefab() {
        return prefab;
    }

    public void setPrefab(String prefab) {
        this.prefab = prefab;
    }

    public Integer getRarity() {
        return rarity;
    }

    public void setRarity(Integer rarity) {
        this.rarity = rarity;
    }

    public Integer getOrdering() {
        return ordering;
    }

    public void setOrdering(Integer ordering) {
        this.ordering = ordering;
    }

    public Integer getSpawnAmount() {
        return spawnAmount;
    }

    public void setSpawnAmount(Integer spawnAmount) {
        this.spawnAmount = spawnAmount;
    }

    public Boolean getSupportsUserText() {
        return supportsUserText;
    }

    public void setSupportsUserText(Boolean supportsUserText) {
        this.supportsUserText = supportsUserText;
    }

    public Long getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(Long categoryId) {
        this.categoryId = categoryId;
    }

    public GameObjectTypeCategory getCategory() {
        return category;
    }

    public void setCategory(GameObjectTypeCategory category) {
        this.category = category;
    }
}

package foundation.softwaredesign.orbi.persistence.entity;

import org.eclipse.persistence.annotations.BatchFetch;
import org.eclipse.persistence.annotations.BatchFetchType;
import org.eclipse.persistence.annotations.ReadOnly;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.io.Serializable;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@ReadOnly
@Table(name = "game_object_type", schema = "public")
public class GameObjectTypeEntity implements Serializable {
    @Id
    private Long id;
    @Column
    private String prefab;
    @Column
    private Boolean supportsUserText;
    @Column(updatable = false, insertable = false)
    private Long categoryId;
    @Column
    private Integer rarity;
    @Column
    private Integer ordering;
    @Column
    private Integer spawnAmount;
    @Column
    @NotNull
    private Boolean ai;
    @ManyToOne
    @BatchFetch(BatchFetchType.JOIN)
    @JoinColumn(name = "category_id")
    private GameObjectTypeCategoryEntity gameObjectTypeCategoryEntity;

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

    public Boolean getSupportsUserText() {
        return supportsUserText;
    }

    public void setSupportsUserText(Boolean supportsUserText) {
        this.supportsUserText = supportsUserText;
    }

    public GameObjectTypeCategoryEntity getGameObjectTypeCategoryEntity() {
        return gameObjectTypeCategoryEntity;
    }

    public void setGameObjectTypeCategoryEntity(GameObjectTypeCategoryEntity gameObjectTypeCategoryEntity) {
        this.gameObjectTypeCategoryEntity = gameObjectTypeCategoryEntity;
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

    public Long getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(Long categoryId) {
        this.categoryId = categoryId;
    }

    public Boolean getAi() {
        return ai;
    }

    public void setAi(Boolean ai) {
        this.ai = ai;
    }
}

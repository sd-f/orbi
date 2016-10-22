package foundation.softwaredesign.orbi.persistence.entity;

import org.eclipse.persistence.annotations.ReadOnly;

import javax.persistence.*;
import java.io.Serializable;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@ReadOnly
@Table(name = "game_object_type_category", schema = "public")
public class GameObjectTypeCategoryEntity implements Serializable {
    @Id
    private Long id;
    @Column
    private String name;
    @Column
    private Boolean craftable;
    @Column
    private Integer rarity;
    @OneToMany(mappedBy = "gameObjectTypeCategoryEntity")
    private List<GameObjectTypeEntity> gameObjectTypeEntities;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Boolean getCraftable() {
        return craftable;
    }

    public void setCraftable(Boolean craftable) {
        this.craftable = craftable;
    }

    public Integer getRarity() {
        return rarity;
    }

    public void setRarity(Integer rarity) {
        this.rarity = rarity;
    }

    public List<GameObjectTypeEntity> getGameObjectTypeEntities() {
        return gameObjectTypeEntities;
    }

    public void setGameObjectTypeEntities(List<GameObjectTypeEntity> gameObjectTypeEntities) {
        this.gameObjectTypeEntities = gameObjectTypeEntities;
    }
}

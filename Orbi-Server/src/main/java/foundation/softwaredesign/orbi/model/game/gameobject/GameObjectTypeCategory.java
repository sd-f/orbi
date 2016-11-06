package foundation.softwaredesign.orbi.model.game.gameobject;

import java.util.ArrayList;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObjectTypeCategory {

    private Long id;
    private String name;
    private Boolean craftable;
    private List<GameObjectType> types = new ArrayList<>();

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

    public List<GameObjectType> getTypes() {
        return types;
    }

    public void setTypes(List<GameObjectType> types) {
        this.types = types;
    }
}

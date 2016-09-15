package foundation.softwaredesign.orbi.persistence.entity;

import org.eclipse.persistence.annotations.ReadOnly;

import javax.persistence.*;
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
}

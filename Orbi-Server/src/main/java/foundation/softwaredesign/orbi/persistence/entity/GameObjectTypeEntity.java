package foundation.softwaredesign.orbi.persistence.entity;

import org.eclipse.persistence.annotations.ReadOnly;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
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
}

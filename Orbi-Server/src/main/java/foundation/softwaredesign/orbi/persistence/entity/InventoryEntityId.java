package foundation.softwaredesign.orbi.persistence.entity;

import org.apache.commons.lang3.builder.EqualsBuilder;
import org.apache.commons.lang3.builder.HashCodeBuilder;

import javax.persistence.Column;
import javax.persistence.Embeddable;
import java.io.Serializable;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Embeddable
public class InventoryEntityId implements Serializable {

    @Column
    private Long gameObjectTypeId;
    @Column
    private Long identityId;

    public InventoryEntityId() {
    }

    public InventoryEntityId(Long gameObjectTypeId, Long identityId) {
        this.gameObjectTypeId = gameObjectTypeId;
        this.identityId = identityId;
    }

    public Long getGameObjectTypeId() {
        return gameObjectTypeId;
    }

    public void setGameObjectTypeId(Long gameObjectTypeId) {
        this.gameObjectTypeId = gameObjectTypeId;
    }

    public Long getIdentityId() {
        return identityId;
    }

    public void setIdentityId(Long identityId) {
        this.identityId = identityId;
    }

    @Override
    public boolean equals(Object o) {
        return EqualsBuilder.reflectionEquals(this,o);
    }

    @Override
    public int hashCode() {
        return HashCodeBuilder.reflectionHashCode(this);
    }
}

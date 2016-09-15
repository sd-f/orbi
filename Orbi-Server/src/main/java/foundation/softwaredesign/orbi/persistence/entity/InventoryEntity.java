package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "inventory", schema = "public")
public class InventoryEntity {

    @EmbeddedId
    private InventoryEntityId id;

    @MapsId("gameObjectTypeId")
    @ManyToOne
    @JoinColumn(name = "game_object_type_id")
    private GameObjectTypeEntity gameObjectType;

    @MapsId("identityId")
    @ManyToOne
    @JoinColumn(name = "identity_id")
    private IdentityEntity identity;

    @Column
    private Long amount;

    public InventoryEntityId getId() {
        return id;
    }

    public void setId(InventoryEntityId id) {
        this.id = id;
    }

    public Long getAmount() {
        return amount;
    }

    public void setAmount(Long count) {
        this.amount = count;
    }

    public GameObjectTypeEntity getGameObjectType() {
        return gameObjectType;
    }

    public void setGameObjectType(GameObjectTypeEntity gameObjectType) {
        this.gameObjectType = gameObjectType;
    }

    public IdentityEntity getIdentity() {
        return identity;
    }

    public void setIdentity(IdentityEntity identity) {
        this.identity = identity;
    }
}

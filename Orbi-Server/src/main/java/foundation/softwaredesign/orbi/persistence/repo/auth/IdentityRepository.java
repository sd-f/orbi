package foundation.softwaredesign.orbi.persistence.repo.auth;

import foundation.softwaredesign.orbi.persistence.entity.IdentityEntity;
import foundation.softwaredesign.orbi.persistence.types.ChkPass;
import org.apache.deltaspike.data.api.AbstractEntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.SingleResultType;

import javax.validation.constraints.NotNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public abstract class IdentityRepository extends AbstractEntityRepository<IdentityEntity, Long> {


    public abstract IdentityEntity findByToken(@NotNull ChkPass token);

    @Query(singleResult = SingleResultType.OPTIONAL)
    public abstract IdentityEntity findByEmail(@NotNull String email);

    @Query(value = "" +
            "SELECT id FROM identity" +
            " WHERE token = ?1", isNative = true)
    public abstract Long findIdentityIdByToken(@NotNull String token);

    @Query(value = "" +
            "SELECT id FROM identity" +
            " WHERE (email = ?1)" +
            "   AND (tmp_password = ?2)", isNative = true)
    public abstract Long findIdentityIdByEmailAndPassword(@NotNull String email, @NotNull String password);

}

package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.persistence.entity.ElevationEntity;
import org.apache.deltaspike.data.api.EntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;

import java.math.BigDecimal;
import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public interface ElevationRepository extends EntityRepository<ElevationEntity, BigInteger> {

    @Query(value = "" +
            "SELECT ST_Value(rast, ST_SetSRID(ST_Point(?2, ?1), 4236))" +
            "  FROM elevation\n" +
            " WHERE longitude = trunc(?2)" +
            "   AND latitude = trunc(?1) ", isNative = true)
    Double getElevation(BigDecimal latitude, BigDecimal longitude);
}
